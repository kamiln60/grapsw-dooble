using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel;
using System.Security.Claims;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using DobbleGameServer.data;
using DobbleGameServer.dto;

namespace DobbleGameServer {
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class DobbleServer : IDobbleServer {
        IGameClientCallback Callback => OperationContext.Current.GetCallbackChannel<IGameClientCallback>();

        private volatile GameState state;

        private BlockingQueue<PlayerPick> picksQueue;

        private BlockingQueue<ConnectionRequest> connectionQueue;

        private List<List<int>> CardSchema;

        private ServerConfigDto Config;

        private Random rng;

        public DobbleServer() {

            this.picksQueue = new BlockingQueue<PlayerPick>();
            this.connectionQueue = new BlockingQueue<ConnectionRequest>();
            this.Config = new ServerConfigDto();

            Console.WriteLine("Generowanie kart dla liczby symboli na każdej: {0} - spodziewana liczba potrzebnych symboli: {1}", Config.SymbolsPerCard, GetNumberOfCards(Config.SymbolsPerCard));

            this.CardSchema = GenerateCardSchema(Config.SymbolsPerCard, out _, out _);
            this.state = new GameState(this.CardSchema);
            //LoadSymbolsFromFiles();

            rng = new Random();

            InitializeThreads();

            Console.WriteLine("Zakończono inicjowanie serwera.");
        }

        public bool Connect(string name) {
            lock (this.state.Players) {
                if (this.state.State != State.LOBBY)
                {
                    Callback.SendLog("Gra jest w toku. Zapraszamy później.");
                    return false;
                }

                if (ContainsName(name)) {
                    Callback.SendLog("Nazwa gracza zajęta, proszę wybrać inną.");
                    return false;
                }

                connectionQueue.Enqueue(new ConnectionRequest(OperationContext.Current, name));

            }
            return true;
        }

        public void PickACard(int token, int symbolNo) {
            lock (state)
            {
                if (state.State != State.PICK_CARD) {
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
            lock (state)
            {
                Console.WriteLine("Przed dodaniem: {0}", token);
                state.Players.Values
                    .ToList().ForEach(player => Console.WriteLine(player.Name));
                lock (this.state.Players) {
                    this.state.Players[token].IsReady = readiness;
                }
            }
            InitGame();
        }

        public void InitGame() {
            lock (state) {
                if (IsRequiredNumberOfPlayers() && IsEveryoneReady() && this.state.State == State.LOBBY) {
                    StartGame();
                }
            }
        }

        public void StartGame() {
            lock (state)
            {
                this.state.State = State.WAIT_FOR_CARD;
                InitializeRound();
                BroadcastMessage("Rozpoczynanie rundy!\n");
            }
        }

        public void InitializeRound() {
            //Niepotrzebne tutaj, bo to akcja automatyczna, nie triggerowana przez użytkownika
            // if (this.state.State != State.WAIT_FOR_CARD) {
            //
            //     return;
            // }

            lock (this.state)
            {
                this.state.CurrentCard = this.state.Cards[rng.Next(1, this.state.Cards.Count)];
                this.state.RoundNumber++;
                if (state.RoundNumber > Config.MaxRoundNumber)
                {
                    StopGame();
                }
                foreach (var playersValue in this.state.Players.Values) {
                    playersValue.CardId = this.state.Cards[rng.Next(1, this.state.Cards.Count)].Id;
                    playersValue.Callback.SendRoundData(new CardRoundDto(
                        this.state.RoundNumber,
                        this.state.CurrentCard,
                        this.state.Cards[playersValue.CardId])
                    );
                }
                this.picksQueue.Clear();
                this.state.State = State.PICK_CARD;
            }

        }

        public void StopGame() {
            lock (this.state)
            {
                this.state.State = State.END;
                BroadcastMessage("Gra zakończona.");
                SendLeaderboard();
                this.state.GameTimer = new Timer(CleanupAfterGame, null, 30000, 0);


                if (state.NextRoundTimer == null) return;
                state.NextRoundTimer.Dispose();
                state.NextRoundTimer = null;
            }
        }

        public void CleanupAfterGame(object arg)
        {
            lock (this.state)
            {
                BroadcastMessage("Zostałeś rozłączony (koniec gry)");
                this.state.Players.Clear();
                state.Players.Values.ToList()
                    .ForEach(player => player.Callback.EndGame());
                this.state = new GameState(CardSchema);
                if (this.state.GameTimer != null)
                {
                    this.state.GameTimer.Dispose();
                    this.state.GameTimer = null;
                }
            }
        }

        public bool Disconnect(int token) {
            lock (this.state.Players) {
                if (this.state.Players.ContainsKey(token)) {
                    return false;
                }
                bool returnValue = Equals(this.state.Players.TryRemove(token, out var player));

                Callback.SendLog("Opuszczono pokój gry.");
                BroadcastMessage(player.Name + " opuścił grę.");
                if (!IsRequiredNumberOfPlayers())
                {
                    BroadcastMessage("Brak wystarczającej liczby graczy do kontynuowania gry.");
                    StopGame();
                }

                return returnValue;
            }

        }

        public Card[] GetCards()
        {
            lock (state)
            {
                return this.state.Cards.Values.ToArray();
            }
        }
    }


}
