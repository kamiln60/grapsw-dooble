using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DobbleGameServer.data;
using DobbleGameServer;
using DobbleGameServer.dto;
using TestClient.DobbleServiceReference;
using TestClient.menu;
using TestClient.menu.command;
using TestClient.remote;

namespace TestClient {
    public class CallbackHandler : IDobbleServerCallback {
        public void LockClient() {
            Console.WriteLine("Blokada");
        }

        public void UnlockClient() {
            Console.WriteLine("Zdjęcie blokady (maszyny losującej HeHeHeHe)");
        }

        public void SendLog(string message)
        {
            Console.WriteLine(message);
        }

        public void SendPlayerData(PlayerDto player)
        {
            Console.Write(player);
            Program.server.Token = player.Id;
        }

        public void SendRoundData(CardRoundDto roundDto)
        {
            Console.WriteLine("RUNDA NR: {0}", roundDto.RoundNumber);
            Console.WriteLine("Karta na stole: ");
            PrintCard(roundDto.CurrentCard);
            Console.WriteLine("Twoja karta: ");
            PrintCard(roundDto.PlayerCard);
            Program.server.Ready = false;
        }

        public void SendLeaderBoard(LeaderboardRow[] leaderboard)
        {
            leaderboard.ToList()
                .ForEach(row => Console.WriteLine("{0} -> {1}", row.Name, row.Points));
        }

        public void EndGame()
        {
            Console.WriteLine("Serwer wywołał metodę zakończenia sesji - kończenie...");
            Program.server.Close();
        }

        public int Ping()
        {
            return Program.server.Token;
        }

        private void PrintCard(Card card)
        {
            Console.Write(card.Id + "=> {");
            card.Symbols.ForEach(symbol => Console.Write(symbol+", "));
            Console.Write("}\n");
        }

        public void SendGameInfo(GameInfo info) {
            Console.WriteLine("{0} jest adminem pokoju.", info.AdminName);
        }

        public void SendPlayerList(PlayerDto[] players) {
            players.ToList().ForEach(player => Console.WriteLine(player.ToString()));
        }

        public void NotifyRoundStart(int round) {
            //throw new NotImplementedException();
        }

        public void NotifyRoundEnd(int round) {
            //throw new NotImplementedException();
        }
        // public void SendCards(Card[] cards) {
        //     foreach (var card in cards) {
        //         Console.WriteLine(card.Id);
        //     }
        // }


    }

    class Program {
        private static InstanceContext context;

        public static Server server { get; set; }



        private static void InitClient() {
            context = new InstanceContext(new CallbackHandler());
            EndpointAddress endpoint = new EndpointAddress("http://192.168.1.2:8000/DobbleServer/");
            WSDualHttpBinding binding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
            //binding.Security.Mode = WSDualHttpSecurityMode.None;
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
            commands.Add(3, new PickCardCommand(server, "Wybierz kartę"));
            commands.Add(4, new ChangeSettingsCommand(server, "Zmień ustawienia"));
            commands.Add(5, new LeaveCommand(server, "Wyjdź z lobby"));
            

            commands.Add(9, new ExitCommand(server, "Wyjdź"));
            menu.AddCommands(commands);

            Console.WriteLine("---------------------");
            Console.WriteLine("Testowy klient Dobble");
            while (true) {
                try {
                    menu.displayMenu();
                    var cmd = int.Parse(Console.ReadLine() ?? string.Empty);
                    menu.execute(cmd);
                    Thread.Sleep(500);
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
