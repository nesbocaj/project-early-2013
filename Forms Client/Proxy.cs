using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCP_Shared;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Forms_Client
{
    class Proxy : ITcpConnection
    {
        private List<String> _cityList = null;

        public Proxy()
        {
            _cityList = new List<string>();
        }

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
    }
}
