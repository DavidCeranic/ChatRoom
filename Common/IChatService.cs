using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    // samo ovaj interfejs je contract, zato i ima ovaj atribut
    //ovo server implementira(klasa ChatService), a client poziva (MainWindow)
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        //metoda koja posalje poruku klijentu koji je samo podignut, a nije poslao poruku
        [OperationContract(IsOneWay = true)]
        void ConnectTo();

        //client poziva na serveru ovu metodu
        [OperationContract(IsOneWay = true)]
        void SendMessageToServer(string message);

        [OperationContract]
        ICryptographyInterface GetCryptoAlg();
    }
}