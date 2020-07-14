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

        public ReaderWriterLockSlim PlayerPickLock { get; set; }

        public ReaderWriterLockSlim ConnectionLock { get; set; }

        public void InitializeThreads() {
            this.PlayerPickThread = new Thread(PlayerPickThreadMethod);
            this.ConnectionManagementThread = new Thread(ConnectionManagementThreadMethod);
            this.PlayerPickLock = new ReaderWriterLockSlim();
            this.ConnectionLock = new ReaderWriterLockSlim();


            this.ConnectionManagementThread.Start();
            this.PlayerPickThread.Start();

        }

        #region Threads

        public void PlayerPickThreadMethod() {
            PlayerPick pick;
            while (true) {


                pick = picksQueue.Dequeue();

                Thread.BeginCriticalRegion();
                PlayerPickLock.EnterWriteLock();
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
                        BroadcastMessage(string.Format("{0}, 3 sekundy odpoczynku! (zły symbol)", state.Players[pick.PlayerToken]));
                        Timer timer = new Timer(UnbanToken, pick.PlayerToken, 5000, System.Threading.Timeout.Infinite);
                        state.BannedTokens.TryAdd(pick.PlayerToken, timer);
                    }
                }

                PlayerPickLock.ExitWriteLock();
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
                ConnectionLock.EnterWriteLock();

                if (this.state.State != State.Lobby) {
                    request.Callback.SendLog("Gra jest w toku. Spróbuj ponownie później.");
                }
                else {
                    Player player = new Player(request.Context, request.Name);
                    int token = GenerateToken();
                    this.state.Players.TryAdd(token, player);
                    player.Callback.SendPlayerData(new PlayerDto(token, player));
                }

                ConnectionLock.ExitWriteLock();
                Thread.EndCriticalRegion();
            }
        }

        #endregion

    }
}
