using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCP_Shared;

namespace Forms_Client
{
    class Proxy : ITcpConnection
    {
        public Response<T> Request<T>(string command)
        {
            return new Response<T>();
        }
    }
}
