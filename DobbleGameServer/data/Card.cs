using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.data
{
    [DataContract]
    public class Card
    {
        [DataMember]
        public int Id { get; set; }

        // [DataMember]
        // public Dictionary<int, Symbol> Symbols;

        [DataMember]
        public List<int> Symbols { get; set; }

        public Card()
        {
            //this.Symbols = new Dictionary<int, Symbol>();
            this.Symbols = new List<int>();
        }
    }
    // [DataContract]
    // [KnownType(typeof(byte[]))]
    // public class Symbol
    // {
    //     [DataMember]
    //     public int Id { get; set; }
    //     [DataMember]
    //     public byte[] Icon { get; set; }
    //     
    //     public Symbol(int id, byte[] icon)
    //     {
    //         Id = id;
    //         Icon = icon;
    //     }
    // }
}
