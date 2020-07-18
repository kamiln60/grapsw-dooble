using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer.data;

namespace DobbleGameServer.dto {
    [DataContract]
    public class PlayerDto {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Points { get; set; }
        [DataMember]
        public int CardId { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }

        [DataMember]
        public bool IsReady { get; set; }

        public PlayerDto(int token, Player player, bool isAdmin)
        {
            this.Id = token;
            this.Name = player.Name;
            this.Points = player.Points;
            this.CardId = player.CardId;
            this.IsReady = player.IsReady;
            this.IsAdmin = isAdmin;
        }

        public PlayerDto(Player player, bool isAdmin) {
            this.Id = 0;
            this.Name = player.Name;
            this.Points = player.Points;
            this.IsReady = player.IsReady;
            this.IsAdmin = isAdmin;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Points)}: {Points}, {nameof(CardId)}: {CardId}";
        }
    }
}
