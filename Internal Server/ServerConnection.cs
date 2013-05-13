using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal_Server
{
    class ServerConnection : IServerConnection
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

        public string Request(string command)
        {
            string response = "";

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
