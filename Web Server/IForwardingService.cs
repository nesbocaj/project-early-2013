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
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/test")]
        string[] Test();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/list/cities")]
        string[] ListCities();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/list/destinations/{initial}")]
        string[] ListDestinations(string initial);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/search?From={initial}&To={destination}")]
        JsonTuple SearchFlight(string initial, string destination);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/watch?From={initial}&To={destination}")]
        void WatchFlight(string initial, string destination);                 
    }

    [DataContract]
    public class JsonTuple
    {
        [DataMember]
        public string[] Item1 { get; set; }

        [DataMember]
        public decimal Item2 { get; set; }
    }
}
