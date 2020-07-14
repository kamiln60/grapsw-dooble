using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.remote;

namespace TestClient.menu.command {
    public class ExitCommand: Command<Server> {
        public ExitCommand(Server remote, string name) : base(remote, name)
        {
            
        }
        
        public override void execute()
        {
            try
            {
                
                remote.Close();
            }
            finally
            {
                Console.WriteLine("Zamykanie...");
                Environment.Exit(0);
            }
            
        }
    }
}
