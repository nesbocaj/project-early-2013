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
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/list/cities")]
        JsonMessage ListCities();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/list/destinations/{initial}")]
        JsonMessage ListDestinations(string initial);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/search?From={initial}&To={destination}")]
        JsonMessage ListSearch(string initial, string destination);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/watch?From={initial}&To={destination}")]
        JsonMessage ListWatch(string initial, string destination);                 
    }

    [DataContract]
    public class JsonMessage
    {
        [DataMember]
        public string Message { get; set; }
    }
}
