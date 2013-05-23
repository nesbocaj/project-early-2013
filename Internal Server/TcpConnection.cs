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
    /// <summary>
    /// Represents the serverside implementation of ITcpConnection
    /// </summary>
    class TcpConnection : ITcpConnection
    {
        private static TcpConnection _instance;
        private CityGraph _graph;

        /// <summary>
        /// Initializes a new instance of the Internal_Server.TcpConnection class
        /// </summary>
        private TcpConnection()
        {
            _graph = CityGraph.Instance;
        }

        /// <summary>
        /// Instance accessor for the Internal_Server.TcpConnection class
        /// </summary>
        public static TcpConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TcpConnection();
                return _instance;
            }
        }

        /// <summary>
        /// Manages the TCP client request
        /// </summary>
        /// <param name="currentSocket">The socket the client is connected to</param>
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

        /// <summary>
        /// Handles a client request based on the specified command
        /// meaning the command is parsed and the appropriate request handler is called. 
        /// Finally a TCP_Shared.Response&lt;T&gt; is generated and serialized for transmission
        /// </summary>
        /// <param name="command">The command specified by the client</param>
        /// <returns>A serialized version of the adequate TCP_Shared.Response&lt;T&gt; object</returns>
        public string Request(string command)
        {
            var serialized = default(string);
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

        /// <summary>
        /// Returns a list of all the commands
        /// </summary>
        /// <returns>A list of all the commands</returns>
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

        /// <summary>
        /// Returns CityGraph.CityList for the CityGraph instance
        /// </summary>
        /// <returns>A list of cities packed in a TCP_Shared.Response&lt;string[]&gt;</returns>
        private Response<string[]> ListCities()
        {
            return new Response<string[]>(
                _graph.Cities,
                "200 OK",
                "The possible cities are the following");
        }

        /// <summary>
        /// Returns CityGraph.Destinations(initial) for the CityGraph instance
        /// </summary>
        /// <param name="initial">The city for which the destinations should be found</param>
        /// <returns>A list of destinations packed in a TCP_Shared.Response&lt;string[]&gt;</returns>
        private Response<string[]> ListDestinations(string initial)
        {
            return new Response<string[]>(
                _graph.ListDestinations(initial),
                "200 OK",
                String.Format("{0} is connected to the following destinations", initial));
        }

        /// <summary>
        /// Returns CityGraph.FindCheapestPath(initial, destination) for the CityGraph instance
        /// </summary>
        /// <param name="initial">The startpoint</param>
        /// <param name="destination">The endpoint</param>
        /// <returns>A list of the traversed waypoints and their cost
        /// packed in a TCP_Shared.Response&lt;&gt;</returns>
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

        /// <summary>
        /// Parses the specified command
        /// </summary>
        /// <param name="command">The recieved command</param>
        /// <returns>A string array consisting of the command identifier and its arguments</returns>
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
