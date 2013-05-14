using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP_Shared;

namespace Internal_Server
{
    class Program
    {
        private TcpConnection _connection;
        private CityGraph _cities;

        private Program()
        {
            GraphInit();
            _connection = TcpConnection.Instance;

            Console.WriteLine("Airline Server");

            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
            listener.Start();
            EndPoint remote = null;

            while (true)
            {
                try
                {
                    var current = listener.AcceptSocket();
                    remote = current.RemoteEndPoint;

                    Console.WriteLine("200 OK - Connection accepted from {0}",
                        remote);

                    var clientThread = new Thread(() => ManageClient(current));
                    clientThread.Start();
                }
                catch (SocketException se)
                {
                    if (remote == null)
                        Console.WriteLine("410 GONE - Connection not established");
                    else
                        Console.WriteLine("410 GONE - Connection with {0} was interrupted",
                            remote);
                }
            }
        }

        static void Main(string[] args)
        {
            new Program();
        }

        public void GraphInit()
        {
            _cities = CityGraph.Instance;
            _cities.InsertVertex(
                "Tirana",
                "Nicosia",
                "Tórshavn",
                "Douglas",
                "Sarajevo",
                "Skopje",
                "Ljubljana",
                "Podgorica",
                "Sofia",
                "Tallinn"
            );
            _cities.ApplyDiscount("Tirana", "Sarajevo");
            _cities.RemoveEdges("Tirana", "Sarajevo");
        }

        void ManageClient(Socket currentSocket)
        {
            NetworkStream stream = new NetworkStream(currentSocket);
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryReader reader = new BinaryReader(stream);

            Response<string> response = null;

            try
            {
                response = _connection.Request<string>(reader.ReadString());
                writer.Write(response.Value);
                writer.Write(response.Code);
                writer.Write(response.Message);
            }
            catch
            {
                Console.WriteLine("400 - BAD REQUEST");
            }

            Console.WriteLine(response.Message);

            stream.Close();
        }
    }
}
