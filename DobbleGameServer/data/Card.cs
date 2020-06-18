using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.data
{
    public class Card
    {
        public int Id { get; set; }


    }

    public class Symbol
    {
        public int Id { get; set; }

        public byte[] Icon { get; set; }
    }
}
