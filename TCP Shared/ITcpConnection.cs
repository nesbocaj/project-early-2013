using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Shared
{
    /// <summary>
    /// Implements the proxypattern
    /// </summary>
    public interface ITcpConnection
    {
        string Request(string command);
    }
}
