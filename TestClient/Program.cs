using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer.data;
using DobbleGameServer;
using TestClient.DobbleServiceReference;
using TestClient.menu;
using TestClient.menu.command;
using TestClient.remote;

namespace TestClient {
    public class CallbackHandler : IDobbleServerCallback {
        public void LockClient() {
            throw new NotImplementedException();
        }

        public void UnlockClient() {
            throw new NotImplementedException();
        }

        public void SendCards(Card[] cards) {
            foreach (var card in cards) {
                Console.WriteLine(card.Id);
            }
        }
    }

    class Program {
        private static InstanceContext context;

        private static Server server;



        private static void InitClient() {
            context = new InstanceContext(new CallbackHandler());
            server = new Server(context);
        }

        static void Main(string[] args) {
            // Context = new InstanceContext(new CallbackHandler());
            // Server client = new Server(Context);
            //
            // Cards = client.GetCards().ToList();
            // Cards.ForEach(card =>
            // {
            //     Console.Write(card.Id);
            //     Console.Write("{");
            //     card.Symbols.ForEach(symbol => Console.Write(symbol + ","));
            //     Console.Write("}\n");
            // });
            InitClient();
            CommandMenu<Server> menu = new CommandMenu<Server>(Program.server);

            var commands = new Dictionary<int, Command<Server>>();
            commands.Add(1, new JoinCommand(server, "Dołącz do lobby"));
            commands.Add(2, new DeclareReadinessCommand(server, "Zaznacz gotowość."));
            commands.Add(3, new JoinCommand(server, "Dołącz do lobby"));


            Console.WriteLine("---------------------");
            Console.WriteLine("Testowy klient Dobble");
            while (true) {
                try {
                    menu.displayMenu();
                    int cmd = int.Parse(Console.ReadLine() ?? string.Empty);
                    menu.execute(cmd);
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }


            }
        }
    }
}
