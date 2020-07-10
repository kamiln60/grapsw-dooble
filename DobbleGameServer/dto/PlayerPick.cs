using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.dto
{
    [DataContract]
    class PlayerPick
    {
        [DataMember]
        public int PlayerToken { get; set; }
        [DataMember]
        public int Pick { get; set; }

        public PlayerPick(int playerToken, int pick)
        {
            PlayerToken = playerToken;
            Pick = pick;
        }
    }
}
