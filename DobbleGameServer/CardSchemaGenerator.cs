using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer.data;

namespace DobbleGameServer
{
    public partial class DobbleServer
    {
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
                    symbolNumbers.Add((symbolsPerCard - 1) + (symbolsPerCard - 1) * (i - 1) + (j + 1));
                }
                cardNumbers.Add(symbolNumbers);
            }

            for (int i = 1; i <= symbolsPerCard - 1; i++)
            {
                for (int j = 1; j <= symbolsPerCard - 1; j++)
                {
                    List<int> symbolNumbers = new List<int>();
                    numberOfCards++;
                    symbolNumbers.Add(i + 1);
                    for (int k = 1; k <= symbolsPerCard - 1; k++)
                    {
                        symbolNumbers.Add((symbolsPerCard + 1) + (symbolsPerCard - 1) * (k - 1) + (((i - 1) * (k - 1) + (j - 1))) % (symbolsPerCard - 1));
                    }
                    cardNumbers.Add(symbolNumbers);
                }
            }

            symbolsCount = cardsCount = cardNumbers.Count;
            return cardNumbers;
        }

        private int GetNumberOfCards(int n)
        {
            return n + (n - 1) * (n - 1);
        }
        private void GenerateCardsFromSchema()
        {
            int cardNo = 1;
            foreach (var card in CardSchema)
            {
                Card cardToAdd = new Card();
                cardToAdd.Id = cardNo++;

                foreach (var symbolId in card)
                {
                    cardToAdd.Symbols.Add(symbolId);
                }

                this.state.Cards.TryAdd(cardToAdd.Id, cardToAdd);
            }
        }
    }
}
