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
            string response = default(string);
            var parsed = ParseCommand(command);
            var commandIdentifier = parsed[0];
            parsed.RemoveAt(0);

            switch(commandIdentifier)
            {
                case "help":
                {
                    break;
                }
                case "list":
                {
                    try
                    {
                        if (parsed[0].ToLower() == "cities")
                            response = ListCities().Serialize();
                        else if (parsed[0].ToLower() == "destinations")
                            response = ListDestinations(parsed[1]).Serialize();
                    }

                    catch (NullReferenceException nre)
                    {
                        // INVALID COMMAND
                        Debug.WriteLine(nre.Message);
                    }

                    break;
                }
                default:
                {
                    // 502 NOT IMPLEMENTED
                    break;
                }
            }

            return response;
        }

        private Response<string[]> ShowHelp()
        {
            return new Response<string[]>(
                _graph.Cities,
                "",
                "");
        }

        private Response<string[]> ListCities()
        {
            return new Response<string[]>(
                _graph.Cities,
                "",
                "");
        }

        private Response<string[]> ListDestinations(string initial)
        {
            return new Response<string[]>(
                _graph.ListDestinations(initial),
                "",
                "");
        }

        private Response<Tuple<string[], decimal>> SearchFlights(string[] arguments)
        {
            return new Response<Tuple<string[], decimal>>();
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
