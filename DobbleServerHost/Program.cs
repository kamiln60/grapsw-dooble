using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer;

namespace DobbleServerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8000/DobbleServer/");
            ServiceHost selfHost = new ServiceHost(typeof(DobbleServer), baseAddress);

            try
            {
                selfHost.AddServiceEndpoint(typeof(IDobbleServer), new WSDualHttpBinding(), "DobbleGameServer");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
                selfHost.Description.Behaviors.Add(smb);

                selfHost.Open();
                Console.WriteLine("Serwis działa....");
                Console.WriteLine("Naciśnij <ENTER> by zakończyć.");
                Console.WriteLine();
                Console.ReadLine();

            }
            catch (CommunicationException e)
            {
                Console.WriteLine("Przechwyciłem wyjątek: {0}", e.Message);
                selfHost.Abort();
            }
        }
    }
}
