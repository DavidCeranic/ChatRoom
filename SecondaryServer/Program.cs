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

namespace SecondaryServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           
            ServiceHost host = new ServiceHost(typeof(SecondaryServer));
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;


            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            host.AddServiceEndpoint(typeof(ISecondaryServer), binding, new Uri("net.tcp://localhost:8888/ISecondaryServer"));
            /// host.Credentials.ServiceCertificate.Certificate = Cer
            //binding.Security.Mode = SecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            ServiceHost host2 = new ServiceHost(typeof(Replication));
            

            host.Open();
            host2.Open();

            Console.WriteLine("Secondary Server is started.");

            Console.ReadLine();
            host.Close();
        }
    }
}