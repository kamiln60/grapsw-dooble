using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer.dto
{
    public class ConnectionRequest
    {
        public OperationContext Context { get; set; }

        public string Name { get; set; }

        public IGameClientCallback Callback => Context.GetCallbackChannel<IGameClientCallback>();

        public ConnectionRequest(OperationContext context, string name)
        {
            Context = context;
            Name = name;
        }
    }
}
