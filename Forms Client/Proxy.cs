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
        public string Request(string command)
        {
            using (var tcp = new TcpClient())
            {
                tcp.Connect(IPAddress.Parse("127.0.0.1"), 7000);

                if (tcp.Connected)
                {
                    var stream = tcp.GetStream();
                    var binReader = new BinaryReader(stream);
                    var binWriter = new BinaryWriter(stream);

                    for (int i = 0; i < 10; i += 1)
                    {
                        binWriter.Write(command);
                        var txt = binReader.ReadString();
                        var response = Response<String[]>.FromSerialized(txt);

                        foreach (var r in response.Value)
                            Debug.WriteLine(r);
                    }
                }
            }
            
            return "";
        }
    }
}
