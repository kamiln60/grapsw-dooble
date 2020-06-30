using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DobbleGameServer.data;

namespace DobbleGameServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DobbleServer : IDobbleServer
    {
        //IGameClientCallback Callback => OperationContext.Current.GetCallbackChannel<IGameClientCallback>();

        private ConcurrentDictionary<string, Player> Players;

        private ConcurrentDictionary<int, Card> Cards;

        private ConcurrentDictionary<int, Symbol> Symbols;

        private List<List<int>> CardSchema;

        private readonly string CARDS_PATH = "C:\\cards";

        private int currentCardId;

        public DobbleServer()
        {
            //this.Players = new List<Player>(); 
            this.Players = new ConcurrentDictionary<string, Player>();
            this.Cards = new ConcurrentDictionary<int, Card>();
            this.Symbols = new ConcurrentDictionary<int, Symbol>();
            if (!Directory.Exists(CARDS_PATH))
            {
                Directory.CreateDirectory(CARDS_PATH);
                Console.WriteLine("Utworzono katalog dla kart.");
            }
            int symbolsPerCard = 8, symbolsCount, cardsCount;
            Console.WriteLine("Generowanie kart dla liczby symboli na każdej: {0} - spodziewana liczba potrzebnych symboli: {1}", symbolsPerCard, GetNumberOfCards(symbolsPerCard));

            this.CardSchema = GenerateCardSchema(symbolsPerCard, out symbolsCount, out cardsCount);

             var filesToLoad = Directory
                 .GetFiles(CARDS_PATH)
                 .Where(IsImage)
                 .ToArray();
             
             int i = 0;
             foreach (var file in filesToLoad)
             {
                 i++;
                 byte[] imageBytes = File.ReadAllBytes(file);
                 this.Symbols.TryAdd(i, new Symbol(i, imageBytes));
             }

        }

        public List<List<int>> GenerateCardSchema(int symbolsPerCard, out int symbolsCount, out int cardsCount)
        {
            int numberOfCards = 0;
            List<List<int>> cardNumbers = new List<List<int>>();
            

            for (int i = 0; i <= symbolsPerCard - 1; i++)
            {
                List<int> symbolNumbers = new List<int>();
                numberOfCards++;
                symbolNumbers.Add(1);
                for (int j = 1; j <= symbolsPerCard - 1; j++)
                {
                    symbolNumbers.Add((symbolsPerCard - 1) + (symbolsPerCard - 1) * (i-1) + (j+1));
                }
                cardNumbers.Add(symbolNumbers);
            }

            for (int i = 1; i <= symbolsPerCard - 1; i++)
            {
                for (int j = 1; j <= symbolsPerCard - 1; j++)
                {
                    List<int> symbolNumbers = new List<int>();
                    numberOfCards++;
                    symbolNumbers.Add(i+1);
                    for (int k = 1; k <= symbolsPerCard - 1; k++)
                    {
                        symbolNumbers.Add((symbolsPerCard + 1) + (symbolsPerCard - 1) * (k-1) + ( ((i-1)*(k-1) + (j-1)))%(symbolsPerCard-1));
                    }
                    cardNumbers.Add(symbolNumbers);
                }
            }

            symbolsCount = cardsCount = cardNumbers.Count;

            return cardNumbers;
        }

        private int GetNumberOfCards(int n)
        {
            return n + (n - 1)*(n - 1);
        }

        private bool IsImage(string name)
        {
            return name.EndsWith(".png") || name.EndsWith(".jpg") || name.EndsWith(".bmp");
        }

        public Player Connect(string name)
        {
            if (Players.ContainsKey(name))
            {
                return null;
            }
            Player player = new Player(OperationContext.Current, name);
            Players.TryAdd(name, player);

            return player;
        }

        public bool Disconnect(string name)
        {
            if (Players.ContainsKey(name))
            {
                return Equals(Players.TryRemove(name, out var player));
            }
            return false;
        }


    }
}
