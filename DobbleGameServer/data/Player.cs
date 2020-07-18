using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.data
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Points { get; set; }

        public IGameClientCallback Callback => Context.GetCallbackChannel<IGameClientCallback>();

        public OperationContext Context { set; get; }

        public int CardId { get; set; }

        public bool IsReady { get; set; }

        public Player(OperationContext context, string name)
        {
            IsReady = false;
            Context = context;
            Name = name;
        }
    }
}
