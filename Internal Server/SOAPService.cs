using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Internal_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SOAPService" in both code and config file together.
    public class SOAPService : ISOAPService
    {

        public string[] Cities()
        {
            return null;
        }

        public string[] Destinations(string from)
        {
            return null;
        }

        public string[] Search(string from, string to)
        {
            return null;
        }

        public string[] Watch(string from, string to)
        {
            return null;
        }
    }
}
