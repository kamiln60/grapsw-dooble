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
        public int Id { get; set; }

        
        public Dictionary<int, Symbol> Symbols;

        public Card()
        {
            this.Symbols = new Dictionary<int, Symbol>();
        }
    }
    [DataContract]
    public class Symbol
    {
        public int Id { get; set; }

        public byte[] Icon { get; set; }

        public Symbol(int id, byte[] icon)
        {
            Id = id;
            Icon = icon;
        }
    }
}
