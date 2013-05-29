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

        /// <summary>
        /// //Constructs and creates instance for SOAP client
        /// </summary>
        public ForwardingService()
        {
            _client = new SOAPConsumer();
        }

        /// <summary>
        /// //Calls method in SOAP client to get Citylist
        /// </summary>
        /// <returns>Returns a string array of city names</returns>
        public string[] ListCities()
        {
            return _client.ListCities();
        }

        /// <summary>
        /// //Calls method in SOAP client to get Destinationlist
        /// </summary>
        /// <param name="from">The name of the initial city to get its possible destinations</param>
        /// <returns>Returns list of possible destination cities</returns>
        public string[] ListDestinations(string from)
        {
            return _client.ListDestinations(from);
        }

        /// <summary>
        /// //Calls method in SOAP client to get flight information
        /// </summary>
        /// <param name="from">The initial city name to obtain flight information</param>
        /// <param name="to">The destination city name to obtain flight information</param>
        /// <returns>Returns flight information</returns>
        public Tuple<string[], decimal> SearchFlight(string from, string to)
        {
            return _client.SearchFlight(from, to);
        }

        /// <summary>
        /// //Calls method in SOAP client to get Watchlist
        /// </summary>
        /// <param name="from">The initial city name</param>
        /// <param name="to">The destination city name</param>
        public void WatchFlight(string from, string to)
        {
            _client.WatchFlight(from, to);
        }
    }
}
