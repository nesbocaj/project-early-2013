using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Shared
{
    public interface ITcpConnection
    {
        Response<T> Request<T>(string command);
    }
}
