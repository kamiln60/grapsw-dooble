using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TestClient.DobbleServiceReference;

namespace TestClient.remote {
    public class Server: DobbleServerClient
    {

        public bool Ready { get; set; } = false;

        public int Token { get; set; } = 0;

        public Server(InstanceContext callbackInstance) : base(callbackInstance)
        {
            
        }

        public Server(InstanceContext callbackInstance, string endpointConfigurationName) : base(callbackInstance, endpointConfigurationName)
        {
        }

        public Server(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public Server(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public Server(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress) : base(callbackInstance, binding, remoteAddress)
        {
        }

        public new void Close()
        {
            if (Token != 0)
            {
                base.Disconnect(Token);
                Token = 0;
            }

            base.Close();
        }
    }
}
