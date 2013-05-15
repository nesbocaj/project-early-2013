using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Internal_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICityGraph" in both code and config file together.
    [ServiceContract]
    public interface ICityGraph
    {
        [OperationContract(IsOneWay = false)]
        string[] Cities();

        [OperationContract(IsOneWay = false)]
        string[] ListDestinations(string from);

        [OperationContract(IsOneWay = false)]
        string[] Search(string from, string to);

        [OperationContract(IsOneWay = false)]
        string[] Watch(string from, string to);
    }
}
