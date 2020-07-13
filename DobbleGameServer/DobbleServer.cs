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
        //private ConcurrentDictionary<int, Symbol> Symbols;
        private GameState state;

        private BlockingQueue<PlayerPick> picksQueue;

        private BlockingQueue<ConnectionRequest> connectionQueue;

        private List<List<int>> CardSchema;

        private Random rng;

        private Timer NextRoundTimer { get; set; }

        private ConcurrentDictionary<int, Timer> BannedTokens;

        //private readonly string CARDS_PATH = "C:\\cards";

        private int currentCardId;

        public DobbleServer() {

            this.picksQueue = new BlockingQueue<PlayerPick>();
            this.connectionQueue = new BlockingQueue<ConnectionRequest>();
            this.BannedTokens = new ConcurrentDictionary<int, Timer>();
            this.state = new GameState();

            //this.Symbols = new ConcurrentDictionary<int, Symbol>();
            // if (!Directory.Exists(CARDS_PATH))
            // {
            //     
            //     Directory.CreateDirectory(CARDS_PATH);
            //     Console.WriteLine("Utworzono katalog dla kart.");
            // }
            int symbolsPerCard = 6, symbolsCount, cardsCount;
            Console.WriteLine("Generowanie kart dla liczby symboli na każdej: {0} - spodziewana liczba potrzebnych symboli: {1}", symbolsPerCard, GetNumberOfCards(symbolsPerCard));

            this.CardSchema = GenerateCardSchema(symbolsPerCard, out symbolsCount, out cardsCount);

            //LoadSymbolsFromFiles();

            GenerateCardsFromSchema();

            rng = new Random();

            InitializeThreads();

            Console.WriteLine("Zakończono inicjowanie serwera.");
        }

        public bool Connect(string name) {
            lock (this.state.Players) {
                if (ContainsName(name)) {
                    Callback.SendLog("Nazwa gracza zajęta, proszę wybrać inną.");
                    return false;
                }

                connectionQueue.Enqueue(new ConnectionRequest(OperationContext.Current, name));

            }
            return true;
        }

        public void PickACard(int token, int symbolNo) {
            if (!state.Players.ContainsKey(token)) {
                return;
            }
            picksQueue.Enqueue(new PlayerPick(token, symbolNo));
        }

        public void DeclareReadiness(int token, bool readiness) {
            Console.WriteLine("Przed dodaniem: {0}", token);
            state.Players.Values
                .ToList().ForEach(player => Console.WriteLine(player.Name));
            lock (this.state.Players)
            {
                this.state.Players[token].IsReady = readiness;
            }
            InitGame();
        }

        public void InitGame()
        {
            lock (this)
            {
                if (IsRequiredNumberOfPlayers() && IsEveryoneReady()) {
                    StartGame();
                }
            }
        }

        public bool IsRequiredNumberOfPlayers()
        {
            return this.state.Players.Keys.Count >= 2;
        }

        public bool IsEveryoneReady()
        {
            foreach (var playersValue in this.state.Players.Values)
            {
                if (!playersValue.IsReady)
                {
                    return false;
                }
            }

            return true;
        }

        public void StartGame()
        {
            this.state.State = State.WAIT_FOR_CARD;
            InitializeRound();
            BroadcastMessage("Rozpoczynanie rundy!\n");
        }

        public void BroadcastMessage(string message)
        {
            this.state.Players
                .Values
                .Select(player => player.Callback)
                .ToList()
                .ForEach(callback => callback.SendLog(message));
        }

        public void InitializeRound()
        {
            if (this.state.State != State.WAIT_FOR_CARD)
            {
                return;
            }

            this.state.CurrentCard = this.state.Cards[rng.Next(1, this.state.Cards.Count)];

            foreach (var playersValue in this.state.Players.Values)
            {
                playersValue.CardId = this.state.Cards[rng.Next(1, this.state.Cards.Count)].Id;
                playersValue.Callback.SendRoundData(new CardRoundDto(this.state.CurrentCard, this.state.Cards[playersValue.CardId]));
            }

            this.state.State = State.PICK_CARD;

        }

        public void StopGame() {

        }

        private int GenerateToken() {
            int tokenToReturn;
            do {
                tokenToReturn = rng.Next();
            } while (this.state.Players.Keys.Contains(tokenToReturn));

            return tokenToReturn;
        }

        private bool ContainsName(string name) {
            List<Player> playersToSearch = this.state.Players.Values.ToList();
            foreach (var player in playersToSearch) {
                if (string.Equals(name, player.Name)) {
                    return true;
                }
            }

            return false;
        }

        public bool Disconnect(int token) {
            lock (this.state.Players) {
                if (this.state.Players.ContainsKey(token)) {
                    return false;
                }
                bool returnValue = Equals(this.state.Players.TryRemove(token, out var player));

                player.Callback.SendLog("dupa");
                return returnValue;
            }

        }

        public string getIdentity() {
            return "NO U";
        }

        public Card[] GetCards() {
            return this.state.Cards.Values.ToArray();
        }

        // private bool IsImage(string name)
        // {
        //     return name.EndsWith(".png") || name.EndsWith(".jpg") || name.EndsWith(".bmp");
        // }
        // private void LoadSymbolsFromFiles()
        // {
        //     var filesToLoad = Directory
        //         .GetFiles(CARDS_PATH)
        //         .Where(IsImage)
        //         .ToArray();
        //
        //     int i = 0;
        //     foreach (var file in filesToLoad)
        //     {
        //         i++;
        //         byte[] imageBytes = File.ReadAllBytes(file);
        //         this.Symbols.TryAdd(i, new Symbol(i, imageBytes));
        //     }
        // }
        // Stara wersja - do przywrócenia jak ogarnięte zostanie przesyłanie obrazków przez sieć
        // private void GenerateCardsFromSchema()
        // {
        //     int cardNo = 1;
        //     foreach (var card in CardSchema)
        //     {
        //         Card cardToAdd = new Card();
        //         cardToAdd.Id = cardNo++;
        //         cardToAdd.Symbols = new Dictionary<int, Symbol>();
        //         foreach (var symbolId in card)
        //         {
        //             Symbol symbolToAdd;
        //             Symbols.TryGetValue(symbolId, out symbolToAdd);
        //             cardToAdd.Symbols.Add(symbolId, symbolToAdd);
        //         }
        //
        //         this.Cards.TryAdd(cardToAdd.Id, cardToAdd);
        //     }
        // }
    }


}
