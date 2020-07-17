using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DobbleGameServer.data;
using DobbleGameServer.dto;

namespace DobbleGameServer {
    public partial class DobbleServer {
        public Thread PlayerPickThread { get; set; }
        public Thread ConnectionManagementThread { get; set; }
        public Thread PingThread { get; set; }

        public ReaderWriterLockSlim RWLock { get; set; }

        public void InitializeThreads() {
            this.PlayerPickThread = new Thread(PlayerPickThreadMethod);
            this.ConnectionManagementThread = new Thread(ConnectionManagementThreadMethod);
            this.PingThread = new Thread(PingThreadMethod);
            this.RWLock= new ReaderWriterLockSlim();


            this.ConnectionManagementThread.Start();
            this.PlayerPickThread.Start();
            this.PingThread.Start();

        }

        #region Threads

        public void PlayerPickThreadMethod() {
            PlayerPick pick;
            while (true) {


                pick = picksQueue.Dequeue();

                Thread.BeginCriticalRegion();
                RWLock.EnterWriteLock();
                if (state.BannedTokens.ContainsKey(pick.PlayerToken)) {
                    state.Players[pick.PlayerToken].Callback.SendLog("Trwa nałożona blokada.");
                }
                else {
                    if (this.state.CurrentCard.Symbols.Contains(pick.Pick) && state.Cards[state.Players[pick.PlayerToken].CardId].Symbols.Contains(pick.Pick)) {
                        this.state.State = State.WaitForReadiness;
                        this.picksQueue.Clear();
                        state.Players[pick.PlayerToken].Points++;
                        BroadcastMessage(string.Format("Koniec rundy {0}, zwyciężył {1}", state.RoundNumber, state.Players[pick.PlayerToken].Name));
                        //this.InitializeRound();
                        BroadcastMessage("Oczekiwanie na gotowość graczy...");
                        state.PlayerList.ForEach(player => player.IsReady = false);
                    }
                    else {
                        state.Players[pick.PlayerToken].Callback.LockClient();
                        BroadcastMessage(string.Format("{0}, 3 sekundy odpoczynku! (zły symbol)", state.Players[pick.PlayerToken].Name));
                        Timer timer = new Timer(UnbanToken, pick.PlayerToken, 5000, System.Threading.Timeout.Infinite);
                        state.BannedTokens.TryAdd(pick.PlayerToken, timer);
                    }
                }

                RWLock.ExitWriteLock();
                Thread.EndCriticalRegion();
            }
        }

        public void UnbanToken(object token) {
            lock (state.BannedTokens) {
                state.BannedTokens[(int)token].Dispose();
                state.BannedTokens.TryRemove((int)token, out _);
                state.Players[(int)token].Callback.SendLog("Zostałeś odblokowany.");
                state.Players[(int)token].Callback.UnlockClient();
            }
        }

        public void ConnectionManagementThreadMethod() {
            ConnectionRequest request;
            while (true) {
                request = connectionQueue.Dequeue();
                Console.WriteLine("Odebrano połączenie od: {0}", request.Name);
                Thread.BeginCriticalRegion();
                RWLock.EnterWriteLock();

                if (this.state.State != State.Lobby) {
                    request.Callback.SendLog("Gra jest w toku. Spróbuj ponownie później.");
                }
                else {
                    
                    Player player = new Player(request.Context, request.Name);
                    int token = GenerateToken();
                    if (state.PlayerList.Count == 0)
                    {
                        this.state.AdminToken = token;
                        player.Callback.SendLog("Zostałeś adminem pokoju.");
                    }
                        
                    this.state.Players.TryAdd(token, player);
                    player.Callback.SendPlayerData(new PlayerDto(token, player, token == this.state.AdminToken));
                }

                RWLock.ExitWriteLock();
                Thread.EndCriticalRegion();
            }
        }

        public void PingThreadMethod() {
            int queueSignal;
            while(true) {
                queueSignal = pingQueue.Dequeue();
                if(pingTimer != null) {
                    PingAllPlayers();
                    pingTimer = new Timer(pingCleanup, null, 5000, 0);
                }
            }
        }

        public void pingCleanup(object arg) {
            if(pingTimer != null) {
                pingTimer.Dispose();
                pingTimer = null;
            }
        }

        #endregion

        public void PingAllPlayers() {
            ISet<int> set = new HashSet<int>();
            foreach (var player in state.PlayerList) {

                try {
                    set.Add(player.Callback.Ping());
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }


            lock (this.state) {
                this.state.Players.Keys.ToList().ForEach(player => {
                    if (!set.Contains(player)) {
                        state.Players.TryRemove(player, out var removedPlayer);
                        BroadcastMessage(string.Format("{0} został usunięty za nieaktywność.", removedPlayer.Name));
                    }

                });

                if (!IsRequiredNumberOfPlayers()) {
                    StopGame();
                }
            }
        }
    }
}
