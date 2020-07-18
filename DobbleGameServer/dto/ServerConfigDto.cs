using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.dto {
    [DataContract]
    public class ServerConfigDto {
        [DataMember]
        public int RoundTimeMS { get; set; } = 60000;
        [DataMember]
        public int RoundIntervalMS { get; set; } = 5000;
        [DataMember] 
        public bool ReadinessEveryRound { get; set; } = true;
        [DataMember] 
        public int MaxRoundNumber { get; set; } = 10;
        [DataMember]
        public int SymbolsPerCard { get; set; } = 6;

        public void AcceptSettings(ServerSettingsDto settings) {
            this.MaxRoundNumber = settings.MaxRoundNumber;
            this.SymbolsPerCard = settings.SymbolsPerCard;
        }
    }
}
