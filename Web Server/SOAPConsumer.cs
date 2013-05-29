using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using Internal_Server;

namespace Web_Server 
{
    public class SOAPConsumer : ClientBase<Internal_Server.ISOAPService>, Internal_Server.ISOAPService
    {

        /// <summary>
        /// //Calls Internal server's SOAP Service to get Citylist
        /// </summary>
        /// <returns>Returns Array of city names from SOAP Service</returns>
        public string[] ListCities()
        {
            return base.Channel.ListCities();
        }

        /// <summary>
        /// //Calls Internal server's SOAP Service to get destinationlist
        /// </summary>
        /// <param name="from">The name of the initial city to get list of destinations</param>
        /// <returns>Returns array of names of destination cities</returns>
        public string[] ListDestinations(string from)
        {
            return base.Channel.ListDestinations(from);
        }

        /// <summary>
        /// //Calls Internal server's SOAP Service to get flight information
        /// </summary>
        /// <param name="from">name of initial city to obtain flight info</param>
        /// <param name="to">name of destination city to obtain flight info</param>
        /// <returns>returns flight information in a tuple</returns>
        public Tuple<string[], decimal> SearchFlight(string from, string to)
        {
            return base.Channel.SearchFlight(from, to);
        }

        /// <summary>
        /// //Calls Internal server's SOAP Service to get watchlist
        /// </summary>
        /// <param name="from">name of initial city</param>
        /// <param name="to">name of destination city</param>
        public void WatchFlight(string from, string to)
        {
            base.Channel.WatchFlight(from, to);
        }
    }
}