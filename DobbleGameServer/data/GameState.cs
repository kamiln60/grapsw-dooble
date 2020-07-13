using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.data {
    public class GameState {
        public ConcurrentDictionary<int, Player> Players { get; set; }
        public ConcurrentDictionary<int, Card> Cards { get; set; }

        public Card CurrentCard { get; set; }

        public State State;

        public GameState() {
            this.Players = new ConcurrentDictionary<int, Player>();
            this.Cards = new ConcurrentDictionary<int, Card>();
            this.State = State.LOBBY;
        }
    }

    public enum State {
        LOBBY,
        PICK_CARD,
        WAIT_FOR_CARD,
        END
    }
}
