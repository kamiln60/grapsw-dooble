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

namespace DobbleGameServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class DobbleServer : IDobbleServer
    {
        IGameClientCallback Callback => OperationContext.Current.GetCallbackChannel<IGameClientCallback>();
 //private ConcurrentDictionary<int, Symbol> Symbols;
        private GameState state;

        private BlockingQueue<PlayerPick> picksQueue;

        private BlockingQueue<ConnectionRequest> connectionQueue;

        private List<List<int>> CardSchema;

        private Random rng;

        //private readonly string CARDS_PATH = "C:\\cards";

        private int currentCardId;

        public DobbleServer()
        {
            
            this.picksQueue = new BlockingQueue<PlayerPick>();
            this.connectionQueue = new BlockingQueue<ConnectionRequest>();
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

             Console.WriteLine("Zakończono inicjowanie serwera.");
        }

        public Player Connect(string name)
        {
            if (ContainsName(name))
            {
                Callback.SendLog("Nazwa gracza zajęta, proszę wybrać inną.");
                return null;
            }

            Player player = new Player(OperationContext.Current, name);
            this.state.Players.TryAdd(GenerateToken(), player);

            return player;
        }

        public void PickCard(int token, int symbolNo)
        {
            if (!this.state.Players.ContainsKey(token))
            {
                return;
            }
            picksQueue.Enqueue(new PlayerPick(token, symbolNo));
        }

        private int GenerateToken()
        {
            int tokenToReturn;
            do
            {
                tokenToReturn = rng.Next();
            } while (this.state.Players.Keys.Contains(tokenToReturn));

            return tokenToReturn;
        }

        private bool ContainsName(string name)
        {
            List<Player> playersToSearch = this.state.Players.Values.ToList();
            foreach (var player in playersToSearch)
            {
                if (string.Equals(name, player.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Disconnect(int token)
        {
            return this.state.Players.ContainsKey(token) && Equals(this.state.Players.TryRemove(token, out var player));
        }

        public string getIdentity()
        {
            return "NO U";
        }

        public Card[] GetCards()
        {
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
