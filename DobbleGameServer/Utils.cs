using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer.data;

namespace DobbleGameServer {
    public partial class DobbleServer {
        public bool IsRequiredNumberOfPlayers() {
            return this.state.Players.Keys.Count >= 2;
        }

        public bool IsEveryoneReady() {
            foreach (var playersValue in this.state.Players.Values) {
                if (!playersValue.IsReady) {
                    return false;
                }
            }

            return true;
        }

        public void BroadcastMessage(string message) {
            try {
                this.state.Players
                    .Values
                    .Select(player => player.Callback)
                    .ToList()
                    .ForEach(callback => callback.SendLog(message));
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public void ExecuteForAllPlayers(Action<Player> action)
        {
            this.state.PlayerList
                .ForEach(action);
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

        private void SendLeaderboard() {
            List<LeaderboardRow> leaderBoard = state.Players.Values
                .Select(player => new LeaderboardRow(player.Name, player.Points))
                .OrderByDescending(player => player.Points)
                .ToList();
            state.Players.Values
                .ToList()
                .ForEach(player => player.Callback.SendLeaderBoard(leaderBoard));
        }

        

        public Card GetRandomUniqueCard() {
            lock(this.state.CardsInUse) {
                Card randomCard = null;

                do {
                    randomCard = this.state.Cards[rng.Next(1, this.state.Cards.Count)];
                } while (state.CardsInUse.Contains(randomCard));
                state.CardsInUse.Add(randomCard);
                return randomCard;
            }
        }

        
    }//End class
    [DataContract]
    public class LeaderboardRow {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Points { get; set; }

        public LeaderboardRow(string name, int points) {
            Name = name;
            Points = points;
        }
    }
}
