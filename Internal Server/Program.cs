using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Internal_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerConnection connection = ServerConnection.Instance;
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);

            listener.Start();

            while (true)
            {
                NetworkStream stream = new NetworkStream(listener.AcceptSocket());
                BinaryWriter writer = new BinaryWriter(stream);
                BinaryReader reader = new BinaryReader(stream);

                string response = connection.Request(reader.ReadString());
                writer.Write(response);
                Console.WriteLine(response);

                stream.Close();
            }
        }
    }
}
