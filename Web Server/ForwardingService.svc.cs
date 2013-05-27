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

        public string[] Test()
        {
            return _client.Test();
        }

        public string[] ListCities()
        {
            return _client.ListCities();
        }

        public string[] ListDestinations(string from)
        {
            return _client.ListDestinations(from);
        }

        public JsonTuple SearchFlight(string from, string to)
        {
            Tuple<string[], decimal> temp = _client.SearchFlight(from, to);

            JsonTuple searchTuple = new JsonTuple();
            searchTuple.Item1 = temp.Item1;
            searchTuple.Item2 = temp.Item2;

            return searchTuple;
        }

        public void WatchFlight(string from, string to)
        {
            _client.WatchFlight(from, to);
        }
    }
}
