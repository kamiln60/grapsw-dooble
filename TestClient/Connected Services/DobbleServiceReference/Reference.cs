﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestClient.DobbleServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DobbleServiceReference.IDobbleServer", CallbackContract=typeof(TestClient.DobbleServiceReference.IDobbleServerCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IDobbleServer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/Connect", ReplyAction="http://tempuri.org/IDobbleServer/ConnectResponse")]
        bool Connect(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/Connect", ReplyAction="http://tempuri.org/IDobbleServer/ConnectResponse")]
        System.Threading.Tasks.Task<bool> ConnectAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/Disconnect", ReplyAction="http://tempuri.org/IDobbleServer/DisconnectResponse")]
        bool Disconnect(int token);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/Disconnect", ReplyAction="http://tempuri.org/IDobbleServer/DisconnectResponse")]
        System.Threading.Tasks.Task<bool> DisconnectAsync(int token);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/GetCards", ReplyAction="http://tempuri.org/IDobbleServer/GetCardsResponse")]
        DobbleGameServer.data.Card[] GetCards();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/GetCards", ReplyAction="http://tempuri.org/IDobbleServer/GetCardsResponse")]
        System.Threading.Tasks.Task<DobbleGameServer.data.Card[]> GetCardsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/PickACard", ReplyAction="http://tempuri.org/IDobbleServer/PickACardResponse")]
        void PickACard(int token, int symbolNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/PickACard", ReplyAction="http://tempuri.org/IDobbleServer/PickACardResponse")]
        System.Threading.Tasks.Task PickACardAsync(int token, int symbolNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/DeclareReadiness", ReplyAction="http://tempuri.org/IDobbleServer/DeclareReadinessResponse")]
        void DeclareReadiness(int token, bool readiness);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDobbleServer/DeclareReadiness", ReplyAction="http://tempuri.org/IDobbleServer/DeclareReadinessResponse")]
        System.Threading.Tasks.Task DeclareReadinessAsync(int token, bool readiness);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDobbleServerCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IDobbleServer/LockClient")]
        void LockClient();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IDobbleServer/UnlockClient")]
        void UnlockClient();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IDobbleServer/SendLog")]
        void SendLog(string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IDobbleServer/SendPlayerData")]
        void SendPlayerData(DobbleGameServer.dto.PlayerDto player);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IDobbleServer/SendRoundData")]
        void SendRoundData(DobbleGameServer.dto.CardRoundDto roundDto);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDobbleServerChannel : TestClient.DobbleServiceReference.IDobbleServer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DobbleServerClient : System.ServiceModel.DuplexClientBase<TestClient.DobbleServiceReference.IDobbleServer>, TestClient.DobbleServiceReference.IDobbleServer {
        
        public DobbleServerClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public DobbleServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public DobbleServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public DobbleServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public DobbleServerClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool Connect(string name) {
            return base.Channel.Connect(name);
        }
        
        public System.Threading.Tasks.Task<bool> ConnectAsync(string name) {
            return base.Channel.ConnectAsync(name);
        }
        
        public bool Disconnect(int token) {
            return base.Channel.Disconnect(token);
        }
        
        public System.Threading.Tasks.Task<bool> DisconnectAsync(int token) {
            return base.Channel.DisconnectAsync(token);
        }
        
        public DobbleGameServer.data.Card[] GetCards() {
            return base.Channel.GetCards();
        }
        
        public System.Threading.Tasks.Task<DobbleGameServer.data.Card[]> GetCardsAsync() {
            return base.Channel.GetCardsAsync();
        }
        
        public void PickACard(int token, int symbolNo) {
            base.Channel.PickACard(token, symbolNo);
        }
        
        public System.Threading.Tasks.Task PickACardAsync(int token, int symbolNo) {
            return base.Channel.PickACardAsync(token, symbolNo);
        }
        
        public void DeclareReadiness(int token, bool readiness) {
            base.Channel.DeclareReadiness(token, readiness);
        }
        
        public System.Threading.Tasks.Task DeclareReadinessAsync(int token, bool readiness) {
            return base.Channel.DeclareReadinessAsync(token, readiness);
        }
    }
}