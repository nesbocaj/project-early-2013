using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Internal_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISOAPService" in both code and config file together.
    [ServiceContract]
    public interface ISOAPService
    {
        [OperationContract(IsOneWay = false)]
        string[] ListCities();

        [OperationContract(IsOneWay = false)]
        string[] ListDestinations(string from);

        [OperationContract(IsOneWay = false)]
        Tuple<string[], decimal> SearchFlight(string from, string to);

        [OperationContract(IsOneWay = true)]
        void WatchFlight(string from, string to);
    }
}
