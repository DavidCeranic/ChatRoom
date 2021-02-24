using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientServerChannel : IDisposable
    {
        public IChatService ProxyChat
        {
            get
            {              
                NetTcpBinding binding = new NetTcpBinding();
                DuplexChannelFactory<IChatService> factory = new DuplexChannelFactory<IChatService>(new InstanceContext(new ChatServiceCallback()), binding, new EndpointAddress("net.tcp://localhost:9999/IChatService"));
                // ChatServiceCallback je klasa koja sluzi za osvjezavanje MainWindowa (ispis svih poslatih poruka na MainWindowu)
                return factory.CreateChannel();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
