using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ISecondaryServer
    {
        //secondary server poziva ovu metodu, a glavni server je implementira
        [OperationContract]
        void SendMessageToSecondaryServer(string message);
    }
}