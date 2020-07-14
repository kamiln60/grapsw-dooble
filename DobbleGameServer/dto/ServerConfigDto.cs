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
        public int RoundTimeMS { get; } = 60000;
        [DataMember]
        public int RoundIntervalMS { get; } = 5000;
        [DataMember] 
        public bool ReadinessEveryRound { get; } = true;
        [DataMember] 
        public int MaxRoundNumber { get; } = 10;
        [DataMember]
        public int SymbolsPerCard { get; } = 6;
    }
}
