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

        public SOAPService()
        {
            _graph = CityGraph.Instance;
        }

        public string[] ListCities()
        {
            return _graph.Cities;
        }

        public string[] ListDestinations(string from)
        {
            return _graph.ListDestinations(from);
        }

        public Tuple<string[], decimal> SearchFlight(string from, string to)
        {
            return _graph.FindCheapestPath(from, to);
        }

        public void WatchFlight(string from, string to)
        {

        }
    }
}
