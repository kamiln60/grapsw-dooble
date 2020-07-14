using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer.data;

namespace DobbleGameServer.dto {
    [DataContract]
    public class CardRoundDto
    {
        [DataMember]
        public Card CurrentCard { get; set; }
        [DataMember]
        public Card PlayerCard { get; set; }
        [DataMember]
        public int RoundNumber { get; set; }
        
        public CardRoundDto(int roundNo, Card currentCard, Card playerCard)
        {
            RoundNumber = roundNo;
            CurrentCard = currentCard;
            PlayerCard = playerCard;
        }
    }
}
