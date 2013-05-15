using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Forms_Client.Model
{
    class Travel
    {
        public string StartingLocation { get; set; }
        public string[] Waypoints { get; set; }
        public string EndLocation { get; set; }
        public int Price { get; set; }
    }
}
