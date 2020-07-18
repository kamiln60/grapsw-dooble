using DobbleGameServer.data;
using DobbleGameServer.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace DobbleGameServer {
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class DobbleServer : IDobbleServer {
        IGameClientCallback Callback => OperationContext.Current.GetCallbackChannel<IGameClientCallback>();

        private volatile GameState state;

        private BlockingQueue<PlayerPick> picksQueue;

        private BlockingQueue<ConnectionRequest> connectionQueue;

        private BlockingQueue<int> pingQueue;

        private Timer pingTimer;

        private List<List<int>> CardSchema;

        private ServerConfigDto Config;

        private Random rng;

        public DobbleServer() {

            this.picksQueue = new BlockingQueue<PlayerPick>();
            this.connectionQueue = new BlockingQueue<ConnectionRequest>();
            this.pingQueue = new BlockingQueue<int>();
            this.Config = new ServerConfigDto();

            Console.WriteLine("Generowanie kart dla liczby symboli na każdej: {0} - spodziewana liczba potrzebnych symboli: {1}", Config.SymbolsPerCard, GetNumberOfCards(Config.SymbolsPerCard));

            this.CardSchema = GenerateCardSchema(Config.SymbolsPerCard, out _, out _);
            rng = new Random();
            this.state = new GameState(this.CardSchema, rng);
            //LoadSymbolsFromFiles();

            

            InitializeThreads();

            Console.WriteLine("Zakończono inicjowanie serwera.");
        }

        public bool Connect(string name) {
            lock (this.state.Players) {
                if (this.state.State != State.Lobby) {
                    Callback.SendLog("Gra jest w toku. Zapraszamy później.");
                    return false;
                }

                if (ContainsName(name)) {
                    Callback.SendLog("Nazwa gracza zajęta, proszę wybrać inną.");
                    return false;
                }
                pingQueue.Enqueue(1);
                connectionQueue.Enqueue(new ConnectionRequest(OperationContext.Current, name));

            }
            return true;
        }

        public void ApplySettings(int token, ServerSettingsDto settings)
        {
            pingQueue.Enqueue(1);
            GameLobbyLock.EnterWriteLock();
                if (!state.Players.ContainsKey(token)) {
                    Callback.SendLog("Gracz nie jest członkiem tej gry!");
                    return;
                }
                if (state.AdminToken != token)
                {
                    Callback.SendLog("Gracz nie ma uprawnień do zmiany ustawień.");
                    return;
                }

                if (state.State != State.Lobby)
                {
                    Callback.SendLog("Zmiana ustawień nie jest możliwa po rozpoczęciu gry.");
                }

                this.Config.AcceptSettings(settings);
                var gameInfo = new GameInfo(state.Players[token].Name, this.Config);
                
                ExecuteForAllPlayers(player =>
                {
                    try {
                        player.Callback.SendLog(string.Format("Administrator pokoju {0} zmienił kilka ustawień.", state.Players[token].Name));
                        player.Callback.SendGameInfo(gameInfo);
                    } catch(Exception e) {
                        Console.WriteLine("Przechwyciłem wyjątek: {0}", e.Message);
                    }
                });
            GameLobbyLock.ExitWriteLock();
            
        }

        public void PickACard(int token, int symbolNo) {

            lock (state) {
                if (state.State != State.PickCard) {
                    Callback.SendLog("Akcja (jeszcze) niedozwolona.");
                    return;
                }
                if (!state.Players.ContainsKey(token)) {
                    Callback.SendLog("Gracz nie jest członkiem tej gry!");
                    return;
                }

            }
            picksQueue.Enqueue(new PlayerPick(token, symbolNo));
        }

        public void DeclareReadiness(int token, bool readiness) {
            if (!state.Players.ContainsKey(token)) {
                Callback.SendLog("Nie jesteś przyłączony do lobby.");
                return;
            }

            GameLobbyLock.EnterWriteLock();

                this.state.Players[token].IsReady = readiness;
                int readyPlayersCount = (from players in state.PlayerList where players.IsReady select players).Count();
                BroadcastMessage(string.Format("{0} zgłosił gotowość! ({1}/{2})", state.Players[token].Name, readyPlayersCount, state.PlayerList.Count));

            GameLobbyLock.ExitWriteLock();
            BroadcastPlayerList();
            InitGame();
        }

        

        public bool Disconnect(int token) {

            lock (this.state) {
                RWLock.EnterWriteLock();
                if (!this.state.Players.ContainsKey(token)) {
                    return false;
                }
                bool returnValue = this.state.Players.TryRemove(token, out var player);

                Callback.SendLog("Opuszczono pokój gry.");
                BroadcastMessage(player.Name + " opuścił grę.");
                BroadcastPlayerList();
                if (!IsRequiredNumberOfPlayers() && state.State != State.Lobby) {
                    BroadcastMessage("Brak wystarczającej liczby graczy do kontynuowania gry.");
                    StopGame();
                }
                if(state.State == State.Lobby) {
                    this.state.Leaderboard.TryRemove(token, out _);
                }
                if(state.Players.Keys.Count == 1 && token == state.AdminToken) {
                    state.AdminToken = this.state.Players.Values.First().Id;
                    state.Players[state.AdminToken].Callback.SendPlayerData(new PlayerDto(state.AdminToken, player, true));
                }
                RWLock.ExitWriteLock();
                return returnValue;
            }

        }

        public Card[] GetCards() {
            lock (state) {
                return this.state.Cards.Values.ToArray();
            }
        }
    }


}
