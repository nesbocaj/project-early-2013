using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal_Server
{
    enum GraphTraversal
    {
        BreadthFirst,
        DepthFirst
    }

    class CityGraph
    {
        private static CityGraph _instance;
        private List<Vertex> _vertices;
        private static Random _randomizer = new Random();

        private CityGraph()
        {
            _vertices = new List<Vertex>();
        }

        public static CityGraph Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CityGraph();
                return _instance;
            }
        }
        public void InsertVertex(params string[] cityNames)
        {
            Array.ForEach(cityNames,
                cityName => _vertices.Add(new Vertex(cityName)));
        }

        public void AddEdge(string fromValue, string toValue)
        {
            Vertex from = _vertices.Find(v => v.CityName.Equals(fromValue));
            Vertex to = _vertices.Find(v => v.CityName.Equals(toValue));

            Edge edge = new Edge(to);

            if (!from.Edges.Any(e => e.Endpoint.Equals(to)))
                from.Add(edge);
        }

        private class Vertex
        {
            private string _cityName;
            private List<Edge> _edges;

            public Vertex(string cityName)
            {
                _cityName = cityName;
                _edges = new List<Edge>();
            }

            public string CityName
            {
                get { return _cityName; }
                set { _cityName = value; }
            }

            public List<Edge> Edges
            {
                get { return _edges; }
            }

            public void Add(Edge edge)
            {
                _edges.Add(edge);
            }
        }

        private class Edge
        {
            private decimal _price;
            private Vertex _endpoint;

            public Edge()
            {
                _price = _randomizer.Next(300, 1000);
            }

            public Edge(Vertex endpoint) : this()
            {
                _endpoint = endpoint;
            }

            public decimal Price
            {
                get { return _price; }
                set { _price = value; }
            }

            public Vertex Endpoint
            {
                get { return _endpoint; }
                set { _endpoint = value; }
            }
        }
    }
}
