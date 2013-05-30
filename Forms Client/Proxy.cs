using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using TCP_Shared;

namespace Forms_Client
{
    /// <summary>
    /// Represents the bit that connects to the server
    /// </summary>
    class Proxy : ITcpConnection
    {
        /// <summary>
        /// Constructs an instance of the proxy
        /// </summary>
        public Proxy() { }

        public bool ObserverState { get; set; }

        public string ObserverResult { get; set; }

        /// <summary>
        /// Handles a client request to the server, 
        /// using "command" as the input message, which determines its behavior
        /// </summary>
        /// <param name="command">The command specified</param>
        /// <returns>An encrypted string</returns>
        public string Request(string command)
        {
            var tcp = new TcpClient();
            string txt = "";
            SocketException ex = null;

            for (int i = 0; i < 10 && !tcp.Connected; i++)
            {
                try
                {
                    tcp.Connect(IPAddress.Parse("127.0.0.1"), 7000);
                    break;
                }
                catch (SocketException se) 
                {
                    ex = se;
                    continue;
                }
                
            }

            if (tcp.Connected)
            {
                var stream = tcp.GetStream();
                var binReader = new BinaryReader(stream);
                var binWriter = new BinaryWriter(stream);

                binWriter.Write(command);
                txt = binReader.ReadString();
            }
            else throw ex;

            return txt;
        }

        public void Post(string command)
        {

        } 
    }
}
