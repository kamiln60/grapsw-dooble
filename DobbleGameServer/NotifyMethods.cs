using DobbleGameServer.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer {
    public partial class DobbleServer {
        public void BroadcastPlayerList() {
            this.ExecuteForAllPlayers(player => {
                player.Callback.SendPlayerList(
                        this.state.PlayerList
                        .Select(playerInstance => new PlayerDto(playerInstance, playerInstance.Id == state.AdminToken))
                        .OrderByDescending(p => p.Points)
                        .ToList()
                    );
            });
        }

        public void NotifyEveryoneAboutRoundStart(int round) {
            this.ExecuteForAllPlayers(player => {
                player.Callback.NotifyRoundStart(round);
            });
        }

        public void NotifyEveryoneAboutRoundEnd(int round) {
            this.ExecuteForAllPlayers(player => {
                player.Callback.NotifyRoundEnd(round);
            });
        }
    }


}
