using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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

                    var clientThread = new Thread(() => _connection.ManageClient(current));
                    clientThread.Start();
                }
                catch (SocketException se)
                {
                    if (remote == null)
                        Console.WriteLine("410 GONE - Connection not established");
                    else
                        Console.WriteLine("410 GONE - Connection with {0} was interrupted",
                            remote);
                    Debug.WriteLine(se.Message);
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
    }
}
