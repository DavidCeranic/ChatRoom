using ChatRoom;
using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ServiceHost host = new ServiceHost(typeof(ChatService));
            NetTcpBinding binding = new NetTcpBinding();
            host.AddServiceEndpoint(typeof(IChatService), binding, new Uri("net.tcp://localhost:9999/IChatService"));
            
            //binding.Security.Mode = SecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            host.Open();


            Console.WriteLine("Server is started.");

            Console.ReadLine();
            host.Close();
        }
    }
}
