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
        public string[] ListCities()
        {
            return base.Channel.ListCities();
        }

        public string[] ListDestinations(string from)
        {
            return base.Channel.ListDestinations(from);
        }

        public Tuple<string[], decimal> SearchFlight(string from, string to)
        {
            return base.Channel.SearchFlight(from, to);
        }

        public void WatchFlight(string from, string to)
        {
            base.Channel.WatchFlight(from, to);
        }
    }
}