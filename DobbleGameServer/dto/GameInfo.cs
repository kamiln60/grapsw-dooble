using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer.data;

namespace DobbleGameServer.dto {
    [DataContract]
    public class GameInfo {
        [DataMember]
        public string AdminName { get; set; }
        [DataMember]
        public ServerConfigDto Config { get; set; }

        public GameInfo(string adminName, ServerConfigDto config)
        {
            AdminName = adminName;
            Config = config;
        }
    }
}
