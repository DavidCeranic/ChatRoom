﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IReplication
    {
        [OperationContract]
        void PosaljiBazu(List<string> baza);

        [OperationContract]
        List<string> PreuzmiBazu();
    }
}
