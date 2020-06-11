using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DoobleGame
{
    public class Player
    {
        public string Name { get; set; }

        public int Points { get; set; }

        public IGameClientCallback Context { get; set; }
    }
}
