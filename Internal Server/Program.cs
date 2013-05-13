using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCP_Shared;

namespace Internal_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ServerConnection.Instance;
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);

            listener.Start();

            while (true)
            {
                var stream = new NetworkStream(listener.AcceptSocket());
                var writer = new BinaryWriter(stream);
                var reader = new BinaryReader(stream);

                var response = connection.Request<string>(reader.ReadString());
                writer.Write(response.Message);
                Console.WriteLine(response);

                stream.Close();
            }
        }
    }
}
