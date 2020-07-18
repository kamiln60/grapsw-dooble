using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.dto {
    [DataContract]
    public class ServerSettingsDto {
        [DataMember]
        public int MaxRoundNumber { get; set; } = 10;
        [DataMember]
        public int SymbolsPerCard { get; set; } = 6;
    }
}
