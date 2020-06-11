using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DoobleGame
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie i pliku konfiguracji.
    public class Service : IService
    {

        private GameData gameData;

        private List<Player> players;

        public Service()
        {
            this.players = new List<Player>();
        }

        public void connect(string playerName)
        {
            var player = new Player(){Name = playerName, Context = Callback, Points = 0};
            players.Add(player);
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

        IGameClientCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IGameClientCallback>();
            }
        }
    }
}
