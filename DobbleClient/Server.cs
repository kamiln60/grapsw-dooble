using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DobbleClient.DobbleServiceReference1;

namespace DobbleClient {
    public class Server: DobbleServerClient
    {
        private static Server instance;
        public bool Ready { get; set; } = false;

        public int Token { get; set; } = 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Server GetInstance()
        {
            if (instance == null)
            {
                InstanceContext ctx = new InstanceContext(DobbleServerCallback.GetInstance());
                instance = new Server(ctx);
                //MessageBox.Show("Interfejs do serwera utworzony");
            }

            return instance;
        }

        private Server(InstanceContext callbackInstance) : base(callbackInstance)
        {
            
        }

        private Server(InstanceContext callbackInstance, string endpointConfigurationName) : base(callbackInstance, endpointConfigurationName)
        {
        }

        private Server(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        private Server(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        private Server(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress) : base(callbackInstance, binding, remoteAddress)
        {
        }

        public new void Close()
        {
            if (Token != 0)
            {
                base.Disconnect(Token);
                Token = 0;
            }
            Thread.Sleep(100);

            base.Close();
        }
    }
}
