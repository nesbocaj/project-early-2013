using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Internal_Server
{
    [ServiceContract]
    public interface ISOAPService
    {
        [OperationContract(IsOneWay = false)]
        string[] ListCities();

        [OperationContract(IsOneWay = false)]
        string[] ListDestinations(string from);

        [OperationContract(IsOneWay = false)]
        string Test();

        [OperationContract(IsOneWay = false)]
        Tuple<string[], decimal> SearchFlight(string from, string to);

        [OperationContract(IsOneWay = true)]
        void WatchFlight(string from, string to);
    }
}
