using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.data
{
    [DataContract]
    public class Player
    {
        public string Name { get; set; }

        public int Points { get; set; }

        private OperationContext _context;

        public IGameClientCallback Callback => _context.GetCallbackChannel<IGameClientCallback>();

        public OperationContext Context
        {
            set => _context = value;
        }

        public Player(OperationContext context, string name)
        {
            _context = context;
            Name = name;
        }
    }
}
