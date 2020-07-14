﻿using System;
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
            try
            {
                this.state.Players
                    .Values
                    .Select(player => player.Callback)
                    .ToList()
                    .ForEach(callback => callback.SendLog(message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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

        public void PingAllPlayers() {
            ISet<int> set = new HashSet<int>();
            foreach (var player in state.PlayerList) {

                try {
                    set.Add(player.Callback.Ping());
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }


            lock (this.state) {
                this.state.Players.Keys.ToList().ForEach(player =>
                {
                    if (!set.Contains(player))
                    {
                        state.Players.TryRemove(player, out var removedPlayer);
                        BroadcastMessage(string.Format("{0} został usunięty za nieaktywność.", removedPlayer.Name));
                    }
                    
                });

                if (!IsRequiredNumberOfPlayers())
                {
                    StopGame();
                }
            }
        }
    }
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
