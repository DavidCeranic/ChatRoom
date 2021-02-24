using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    //ovo client implementira (u klasi ChatServiceCallback) a server poziva (metoda BroadcastMessage u GroupsHelper klasi)
    public interface IChatServiceCallback
    {
        //server poziva na clientu ovu metodu
        [OperationContract(IsOneWay = true)]
        void SendMessageToClients(string message);
    }
}
