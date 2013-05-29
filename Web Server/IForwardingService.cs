using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Web_Server
{
    [ServiceContract]
    public interface IForwardingService
    {

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/list/cities")]
        string[] ListCities();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/list/destinations/{initial}")]
        string[] ListDestinations(string initial);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/search?From={initial}&To={destination}")]
        Tuple<string[], decimal> SearchFlight(string initial, string destination);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/watch?From={initial}&To={destination}")]
        void WatchFlight(string initial, string destination);                 
    }
}
