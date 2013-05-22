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