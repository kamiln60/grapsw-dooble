using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DobbleGameServer.command;
using DobbleGameServer.data;

namespace DobbleGameServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DobbleServer : IDobbleServer
    {
        //IGameClientCallback Callback => OperationContext.Current.GetCallbackChannel<IGameClientCallback>();

        private ConcurrentDictionary<string, Player> players;

        public DobbleServer()
        {
            //this.players = new List<Player>(); 
            this.players = new ConcurrentDictionary<string, Player>();
        }

        public Player connect(string name)
        {
            if (players.ContainsKey(name))
            {
                return null;
            }
            Player player = new Player(OperationContext.Current, name);
            players.TryAdd(name, player);

            return player;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
