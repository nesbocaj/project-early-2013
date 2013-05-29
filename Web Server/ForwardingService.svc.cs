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
    public class ForwardingService : IForwardingService
    {
        private SOAPConsumer _client;
        private JavaScriptSerializer _serializer;

        public ForwardingService()
        {
            _client = new SOAPConsumer();
        }

        public string[] ListCities()
        {
            return _client.ListCities();
        }

        public string[] ListDestinations(string from)
        {
            return _client.ListDestinations(from);
        }

        public Tuple<string[], decimal> SearchFlight(string from, string to)
        {
            return _client.SearchFlight(from, to);
        }

        public void WatchFlight(string from, string to)
        {
            _client.WatchFlight(from, to);
        }
    }
}
