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

        public string[] Cities
        {
            get { return _vertices.Select(v => v.CityName).ToArray(); }
        }

        public void InsertVertex(params string[] cityNames)
        {
            Array.ForEach(cityNames,
                cityName => 
                {
                    var temp = new Vertex(cityName);
                    AddEdges(temp);
                    _vertices.Add(temp);
                });

        }

        private void AddEdges(Vertex vertex)
        {
            foreach (var temp in _vertices)
            {
                decimal price = _randomizer.Next(300, 1000);

                if (!vertex.Edges.Exists(e => e.Endpoint.Equals(temp)))
                    vertex.Edges.Add(new Edge(temp, price));

                if (!temp.Edges.Exists(e => e.Endpoint.Equals(vertex)))
                    temp.Edges.Add(new Edge(vertex, price));
            }
        }

        public void RemoveEdges(string waypointA, string waypointB)
        {
            var vertexA = _vertices
                .Find(v => v.CityName.Equals(waypointA, StringComparison.InvariantCultureIgnoreCase));
            var vertexB = _vertices
                .Find(v => v.CityName.Equals(waypointB, StringComparison.InvariantCultureIgnoreCase));

            int indexA = vertexA.Edges.FindIndex(e => e.Endpoint.Equals(vertexB));
            vertexA.Edges.RemoveAt(indexA);

            int indexB = vertexB.Edges.FindIndex(e => e.Endpoint.Equals(vertexA));
            vertexB.Edges.RemoveAt(indexB);
        }

        public string[] ListDestinations(string initial)
        {
            return _vertices
                .Select(v => v.CityName)
                .Where(s => !s.Equals(initial, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
        }

        public void ApplyDiscount(string waypointA, string waypointB)
        {
            decimal price = _randomizer.Next(100, 200);

            var vertexA = _vertices
                .Find(v => v.CityName.Equals(waypointA, StringComparison.InvariantCultureIgnoreCase));
            var vertexB = _vertices
                .Find(v => v.CityName.Equals(waypointB, StringComparison.InvariantCultureIgnoreCase));

            vertexA.Edges.Single(e => e.Endpoint.Equals(vertexB)).Price = price;
            vertexB.Edges.Single(e => e.Endpoint.Equals(vertexA)).Price = price;
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

            public Edge() { }

            public Edge(Vertex endpoint, decimal price) : this()
            {
                _endpoint = endpoint;
                _price = price;
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
