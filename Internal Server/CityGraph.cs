using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Tuple<string[], decimal> FindCheapestPath(string initial, string destination)
        {
            var resultingPath = new List<Vertex>();
            var priorityQueue = new List<Vertex>();

            var initialVertex = _vertices
                .Find(v => v.CityName.Equals(initial, StringComparison.InvariantCultureIgnoreCase));
            initialVertex.Visited = true;
            initialVertex.PriceFromInitialVertex = 0;

            priorityQueue.Add(initialVertex);
            var frontVertex = initialVertex;
            bool searching = true;

            for (int counter = 0; priorityQueue.Count > 0 && searching; counter++)
            {
                decimal value = 1000000000;
                int i = 0;

                foreach (var v in priorityQueue)
                {
                    if (v.PriceFromInitialVertex < value)
                    {
                        frontVertex = priorityQueue[i];
                        value = v.PriceFromInitialVertex;
                    }

                    i++;

                }

                frontVertex.Visited = true;
                priorityQueue.Remove(frontVertex);

                if (frontVertex.CityName.Equals(destination, StringComparison.InvariantCultureIgnoreCase))
                {
                    var temp = frontVertex;

                    while (!temp.CityName.Equals(initial, StringComparison.InvariantCultureIgnoreCase))
                    {
                        resultingPath.Add(temp);
                        temp = temp.Previous;
                    }

                    searching = false;
                    break;
                }

                foreach (var e in frontVertex.Edges)
                {
                    var price = e.Price;
                    if (!e.Endpoint.Visited)
                    {
                        var isFound = false;
                        var priceIsLess = false;

                        foreach (var v in priorityQueue)
                        {
                            if (v.CityName.Equals(e.Endpoint.CityName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                isFound = true;

                                if (frontVertex.PriceFromInitialVertex + price < v.PriceFromInitialVertex)
                                {
                                    priceIsLess = true;
                                }
                                else
                                {
                                    priceIsLess = false;
                                    Debug.WriteLine("");
                                }
                            }
                            else
                                isFound = false;
                        }

                        if (isFound && priceIsLess)
                        {
                            var tmp = e.Endpoint;
                            tmp.PriceFromInitialVertex = frontVertex.PriceFromInitialVertex + price;
                            tmp.Previous = frontVertex;
                        }
                        else if (isFound && !priceIsLess)
                        {
                            
                        }
                        else if (!isFound && !priceIsLess)
                        {
                            e.Endpoint.PriceFromInitialVertex = frontVertex.PriceFromInitialVertex + price;
                            e.Endpoint.Previous = frontVertex;
                            priorityQueue.Add(e.Endpoint);
                        }
                    }
                    else
                    {
                        // debug message
                    }
                }

                Debug.WriteLine(counter);
            }

            var waypoints = resultingPath.Select(v => v.CityName).ToArray();
            return new Tuple<string[], decimal>(waypoints, frontVertex.PriceFromInitialVertex);
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
            private bool _visited;
            private decimal _priceFromInitialVertex;
            private Vertex _previous;

            public Vertex(string cityName)
            {
                _cityName = cityName;
                _edges = new List<Edge>();
                _visited = false;
                _priceFromInitialVertex = 0;
            }

            public string CityName
            {
                get { return _cityName; }
                set { _cityName = value; }
            }

            public bool Visited
            {
                get { return _visited; }
                set { _visited = value; }
            }

            public decimal PriceFromInitialVertex
            {
                get { return _priceFromInitialVertex; }
                set { _priceFromInitialVertex = value; }
            }

            public Vertex Previous
            {
                get { return _previous; }
                set { _previous = value; }
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
