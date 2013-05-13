using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCP_Shared;

namespace Internal_Server
{
    class ServerConnection : ITcpConnection
    {
        private static ServerConnection _instance;

        private ServerConnection() { }

        public static ServerConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServerConnection();
                return _instance;
            }
        }

        public Response<T> Request<T>(string command)
        {
            Response<T> response = new Response<T>();

            return response;
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
