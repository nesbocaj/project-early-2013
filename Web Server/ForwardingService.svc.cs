using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Internal_Server;
using System.Web.Script.Serialization;

namespace Web_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ForwardingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ForwardingService.svc or ForwardingService.svc.cs at the Solution Explorer and start debugging.
    public class ForwardingService : IForwardingService
    {
        private SOAPConsumer _client;
        private JavaScriptSerializer _serializer;

        public ForwardingService()
        {
            _client = new SOAPConsumer();
            _serializer = new JavaScriptSerializer();
        }

        public JsonMessage TestMethod()
        {
            JsonMessage cityList = new JsonMessage();
            cityList.Message = "[{index:'0',name:'Copenhagen'},{index:'1',name:'Beijing'}]";

            return cityList;
        }


        public JsonMessage ListCities()
        {                           
            
            JsonMessage cityList = new JsonMessage();
            cityList.Message = _serializer.Serialize(_client.ListCities());

            return cityList;
        }

        public JsonMessage ListDestinations(string from)
        {
            JsonMessage destinationList = new JsonMessage();
            destinationList.Message = _serializer.Serialize(_client.ListDestinations(from));

            return destinationList;
        }

        public JsonMessage SearchFlight(string from, string to)
        {
            JsonMessage searchList = new JsonMessage();
            searchList.Message = _serializer.Serialize(_client.SearchFlight(from, to));

            return searchList;
        }

        public void WatchFlight(string from, string to)
        {
            _client.WatchFlight(from, to);
        }
    }
}
