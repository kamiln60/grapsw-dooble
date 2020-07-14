using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DobbleGameServer.data {
    public class GameState {
        public ConcurrentDictionary<int, Player> Players { get; set; }
        public ConcurrentDictionary<int, Card> Cards { get; set; }

        public Timer NextRoundTimer { get; set; }

        public Timer GameTimer { get; set; }

        public ConcurrentDictionary<int, Timer> BannedTokens { get; set; }

        public Card CurrentCard { get; set; }

        public int RoundNumber { get; set; } = 0;

        public State State { get; set; }

        public List<Player> PlayerList => Players.Values.ToList();

        public GameState(List<List<int>> cardSchema) {
            this.Players = new ConcurrentDictionary<int, Player>();
            this.Cards = new ConcurrentDictionary<int, Card>();
            this.BannedTokens = new ConcurrentDictionary<int, Timer>();
            this.State = State.Lobby;

            GenerateCardsFromSchema(cardSchema);
        }

        public void GenerateCardsFromSchema(List<List<int>> CardSchema) {
            int cardNo = 1;
            foreach (var card in CardSchema) {
                Card cardToAdd = new Card();
                cardToAdd.Id = cardNo++;

                foreach (var symbolId in card) {
                    cardToAdd.Symbols.Add(symbolId);
                }

                this.Cards.TryAdd(cardToAdd.Id, cardToAdd);
            }
        }
    }

    public enum State {
        Lobby,
        WaitForReadiness,
        PickCard,
        WaitForCard,
        End
    }
}
