using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Internal_Server
{
    public class SOAPService : ISOAPService
    {
        private CityGraph _graph;

        /// <summary>
        /// //Gets instance of CityGraph
        /// </summary>
        public SOAPService()
        {
            _graph = CityGraph.Instance;
        }

        /// <summary>
        /// //Calls CityGraph to get citylist
        /// </summary>
        /// <returns>Returns an array of city names</returns>
        public string[] ListCities()
        {
            return _graph.Cities;
        }

        /// <summary>
        /// //Calls CityGraph to get Destinationlist
        /// </summary>
        /// <param name="from">The initial city name to obtain destination list</param>
        /// <returns>returns an array of destinations</returns>
        public string[] ListDestinations(string from)
        {
            return _graph.ListDestinations(from);
        }

        /// <summary>
        /// //Calls CityGraph to get flight information
        /// </summary>
        /// <param name="from">The initial city name to get its flight info</param>
        /// <param name="to">The destination city name get its flight info</param>
        /// <returns>Returns a list of waypoint cities and the total price, in a tuple</returns>
        public Tuple<string[], decimal> SearchFlight(string from, string to)
        {
            return _graph.FindCheapestPath(from, to);
        }

        /// <summary>
        /// //Calls CityGraph to get Watchlist?
        /// </summary>
        /// <param name="from">The initial city name</param>
        /// <param name="to">The destination city name</param>
        public void WatchFlight(string from, string to)
        {
            //Does Nothings
        }
    }
}
