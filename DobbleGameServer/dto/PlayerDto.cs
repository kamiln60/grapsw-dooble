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

        public PlayerDto(int token, Player player)
        {
            this.Id = token;
            this.Name = player.Name;
            this.Points = player.Points;
            this.CardId = player.CardId;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Points)}: {Points}, {nameof(CardId)}: {CardId}";
        }
    }
}
