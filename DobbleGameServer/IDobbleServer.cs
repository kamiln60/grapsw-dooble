using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DobbleGameServer.data;

namespace DobbleGameServer
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę interfejsu „IService1” w kodzie i pliku konfiguracji.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IGameClientCallback))]
    public interface IDobbleServer
    {
        [OperationContract]
        Player Connect(string name);

        [OperationContract]
        bool Disconnect(int token);

        [OperationContract]
        Card[] GetCards();

        void PickCard(int token, int symbolNo);

        // TODO: dodaj tutaj operacje usługi
    }

    // Użyj kontraktu danych, jak pokazano w poniższym przykładzie, aby dodać typy złożone do operacji usługi.
    // Możesz dodać pliki XSD do projektu. Po skompilowaniu projektu możesz bezpośrednio użyć zdefiniowanych w nim typów danych w przestrzeni nazw „DobbleGameServer.ContractType”.

    public interface IGameClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void LockClient();
        [OperationContract(IsOneWay = true)]
        void UnlockClient();

        [OperationContract(IsOneWay = true)]
        void SendLog(string message);
    }
}
