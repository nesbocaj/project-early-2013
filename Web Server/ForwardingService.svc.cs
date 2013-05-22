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
        private SOAPConsumer client;

        public ForwardingService()
        {
            client = new SOAPConsumer();
        }

        public JsonMessage TestMethod()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonMessage cityList = new JsonMessage();
            cityList.Message = "[{index:'0',name:'Copenhagen'},{index:'1',name:'Beijing'}]";

            return cityList;
        }


        public JsonMessage Cities()
        {                           
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonMessage cityList = new JsonMessage();
            cityList.Message = serializer.Serialize(client.Cities());

            return cityList;
        }

        public JsonMessage Destinations(string from)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonMessage destinationList = new JsonMessage();
            destinationList.Message = serializer.Serialize(client.Destinations(from));

            return destinationList;
        }

        public JsonMessage Search(string from, string to)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonMessage searchList = new JsonMessage();
            searchList.Message = serializer.Serialize(client.Search(from, to));

            return searchList;
        }

        public JsonMessage Watch(string from, string to)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonMessage WatchList = new JsonMessage();
            WatchList.Message = serializer.Serialize(client.Watch(from, to));

            return WatchList;
        }
    }
}
