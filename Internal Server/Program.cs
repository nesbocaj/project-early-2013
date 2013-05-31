using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP_Shared;

namespace Internal_Server
{
    class Program
    {
        private TcpConnection _connection;
        private CityGraph _graph;
        private Thread _discountWorker, _removeWorker, _listenWorker;

        private Program()
        {
            GraphInit();
            _connection = TcpConnection.Instance;

            Console.WriteLine("Airline Server");

            var soapHost = new ServiceHost(typeof(SOAPService));
            soapHost.BeginOpen(null, soapHost);

            _discountWorker = new Thread(() => ApplyDiscounts());
            _discountWorker.Name = "Discount Worker";
            _discountWorker.Start();

            _removeWorker = new Thread(ApplyDiscounts);
            _removeWorker.Name = "Remove Worker";
            _removeWorker.Start();

            _listenWorker = new Thread(Listen);
            _listenWorker.Name = "Listen Worker";
            _listenWorker.Start();
        }

        static void Main(string[] args)
        {
            new Program();
        }

        private void GraphInit()
        {
            _graph = CityGraph.Instance;
            _graph.InsertVertex(
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
            _graph.ApplyDiscount("Tirana", "Sarajevo");
            _graph.RemoveEdges("Tirana", "Sarajevo");
            var result = _graph.FindCheapestPath("Tirana", "Nicosia");
            var result2 = _graph.FindCheapestPath("Tirana", "Sofia");
        }

        private void Listen()
        {
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

        /// <summary>
        /// Applies discount to edges randomly every minute
        /// </summary>
        private void ApplyDiscounts()
        {
            Thread.Sleep(1000 * 60);

            while (true)
            {
                var result = _graph.ApplyDiscountRandomly();

                var response = new Response<Tuple<string, string, decimal>>(
                    result,
                    "200 OK",
                    "");

                var serialized = response.Serialize();
                _connection.Broadcast(serialized);

                Thread.Sleep(1000 * 60);
            }
        }

        /// <summary>
        /// Removes edges randomly every minute
        /// </summary>
        private void RemoveEdges()
        {
            while (true)
            {
                _graph.ApplyDiscountRandomly();
                Thread.Sleep(1000 * 60);
            }
        }
    }
}
