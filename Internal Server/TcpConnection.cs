using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCP_Shared;

namespace Internal_Server
{
    class TcpConnection : ITcpConnection
    {
        private static TcpConnection _instance;
        private CityGraph _graph;

        private TcpConnection()
        {
            _graph = CityGraph.Instance;
        }

        public static TcpConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TcpConnection();
                return _instance;
            }
        }

        public void ManageClient(Socket currentSocket)
        {
            var stream = new NetworkStream(currentSocket);
            var reader = new BinaryReader(stream);
            var writer = new BinaryWriter(stream);

            try
            {
                string command = reader.ReadString();
                writer.Write(Request(command));
            }
            catch (Exception ex)
            {
                Console.WriteLine("400 - BAD REQUEST");
                Debug.WriteLine(ex.Message);
            }

            stream.Close();
        }

        /*
         * help
         * list cities
         * list destinations
         * search flights <initial city> <destination city>
         * watch flight <waypoint cities, ...>
         */

        public string Request(string command)
        {
            string serialized = default(string);
            var parsed = ParseCommand(command);
            var commandIdentifier = parsed[0];
            parsed.RemoveAt(0);

            switch (commandIdentifier)
            {
                case "help":
                {
                    ShowHelp();
                    break;
                }
                case "list":
                {
                    try
                    {
                        if (parsed[0].ToLower() == "cities")
                        {
                            var response = ListCities();
                            Console.WriteLine(response);
                            serialized = response.Serialize();
                        }
                        else if (parsed[0].ToLower() == "destinations")
                        {
                            var response = ListDestinations(parsed[1]);
                            Console.WriteLine(response);
                            serialized = response.Serialize();
                        }
                    }

                    catch (NullReferenceException nre)
                    {
                        Debug.WriteLine(nre.Message);
                        var response = new Response<string[]>(
                            "400 BAD REQUEST",
                            string.Format("Could not {0}", command),
                            false);
                        Console.WriteLine(response);
                        serialized = response.Serialize();
                    }

                    break;
                }
                case "search":
                {
                    serialized = SearchFlights(parsed[1], parsed[2]).Serialize();
                    break;
                }
                case "watch":
                {
                    break;
                }
                default:
                {
                    var response = new Response<object>(
                            "502 COMMAND NOT IMPLEMENTED",
                            string.Format("Could not {0}", command),
                            false);
                    Console.WriteLine(response);
                    serialized = response.Serialize();

                    break;
                }
            }

            return serialized;
        }

        private Response<string[]> ShowHelp()
        {
            return new Response<string[]>(
                new [] {
                    "help",
                    "list cities",
                    "list destinations <initial city>",
                    "search flights <initial city> <destination city>",
                    "watch flight <waypoint cities, ...>"
                },
                "200 OK",
                "The following commands are available");
        }

        private Response<string[]> ListCities()
        {
            return new Response<string[]>(
                _graph.Cities,
                "200 OK",
                "The possible cities are the following");
        }

        private Response<string[]> ListDestinations(string initial)
        {
            return new Response<string[]>(
                _graph.ListDestinations(initial),
                "200 OK",
                String.Format("{0} is connected to the following destinations", initial));
        }

        private Response<Tuple<string[], decimal>> SearchFlights(string initial, string destination)
        {
            var result = _graph.FindCheapestPath(initial, destination);

            if (result != null)
                return new Response<Tuple<string[], decimal>>(
                    result,
                    "200 OK",
                    String.Format("Your flights from {0} to {1} is as follows", initial, destination));
            else
                return new Response<Tuple<string[], decimal>>(
                    "400 BAD REQUEST",
                    String.Format("Could not find a path between {0} and {1}", initial, destination),
                    false);
        }

        private void WatchFlight(string[] arguments)
        {
            
        }

        private List<string> ParseCommand(string command)
        {
            bool inQuotes = false;
            int i = 0;
            List<string> arguments = new List<string>();
            arguments.Add("");

            foreach (char c in command)
            {
                switch (c)
                {
                    case ' ':
                        if (!inQuotes)
                        {
                            arguments.Add("");
                            i++;
                        }
                        break;
                    case '"':
                        if (!inQuotes)
                            inQuotes = true;
                        else
                            inQuotes = false;
                        break;
                    default:
                        arguments[i] += c;
                        break;
                }
            }

            return arguments;
        }
    }
}
