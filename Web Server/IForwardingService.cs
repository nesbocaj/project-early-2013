using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Web_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IForwardingService" in both code and config file together.
    [ServiceContract]
    public interface IForwardingService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/list/cities")]
        string[] Cities();

        [OperationContract]
        [WebGet(UriTemplate = "/list/destinations/{initial}")]
        string[] Destination(string initial);

        [OperationContract]
        [WebGet(UriTemplate = "/search?From={initial}&To={destination}")]
        string[] Search(string initial, string destination);

        [OperationContract]
        [WebGet(UriTemplate = "/watch?From={initial}&To={destination}")]
        string[] Watch(string initial, string destination);
    }
}
