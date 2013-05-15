using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Internal_Server;

namespace Web_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ForwardingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ForwardingService.svc or ForwardingService.svc.cs at the Solution Explorer and start debugging.
    public class ForwardingService : 
        ClientBase<Internal_Server.ICityGraph>, 
        Internal_Server.ICityGraph, 
        IForwardingService
    {
        public string[] Cities()
        {
            return base.Channel.Cities();
        }

        public string[] Destination(string initial)
        {
            return base.Channel.Destination();
        }

        public string[] Search(string initial, string destination)
        {
            return base.Channel.Search();
        }

        public string[] Watch(string initial, string destination)
        {
            return base.Channel.Watch();
        }
    }
}
