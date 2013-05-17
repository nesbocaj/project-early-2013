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
    public class ForwardingService : 
        ClientBase<Internal_Server.ISOAPService>, 
        Internal_Server.ISOAPService, 
        IForwardingService
    {
        public JsonMessage ListCities()
        {                           
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonMessage cityList = new JsonMessage();
            cityList.Message = serializer.Serialize(base.Channel.Cities());

            return cityList;
        }

        public JsonMessage ListDestinations(string initial)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonMessage destinationList = new JsonMessage();
            destinationList.Message = serializer.Serialize(base.Channel.Destinations(initial));

            return destinationList;
        }

        public JsonMessage ListSearch(string initial, string destination)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
           // string destinationList = serializer.Serialize();

            return null; //base.Channel.Search();
        }

        public JsonMessage ListWatch(string initial, string destination)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
           // string destinationList = serializer.Serialize();

            return null; // base.Channel.Watch();
        }

        public string[] Cities()
        {
            return base.Channel.Cities();
        }

        public string[] Destinations(string from)
        {
            return base.Channel.Destinations(from);
        }

        public string[] Search(string from, string to)
        {
            return base.Channel.Search(from, to);
        }

        public string[] Watch(string from, string to)
        {
            return base.Channel.Watch(from, to);
        }
    }
}
