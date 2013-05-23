using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal_Server
{
	/// <summary>
	/// GraphTraversal klasse indeholder en  BreadthFirst og DepthFirst som bruges til at printe vertices og edges
	///  => sådan at rækkefølgen stemmer overens med at den korteste vej mellem to punkter findes;
	/// BreadthFirst bruger en Queue list som elementer indsættes i;
	/// DepthFirst bruger Stack når der pushes elementer i den;
	/// </summary>
	 
	enum GraphTraversal
	{
		BreadthFirst,
		DepthFirst
	}
	/// <summary>
	///_instance er en singleton instannce og da den er static får den global adgang til alle andre objekter i klassen;
	/// _vertices består af en list af vertices værdier;
	/// Random indeholder tilfældige int værdier fra min til max og her bruges den i forbindelse med prisen;
	///lastinitial er væriden til den sidste element i graphen når loop løber igennem det hele;
	/// </summary>

	class CityGraph
	{
		private static CityGraph _instance;
		private List<Vertex> _vertices;
		private static Random _randomizer = new Random();
		private string lastInitial;
		/// <summary>
		/// Her oprettes Constructor CityGraph()
		/// Constructor giver dens variable en start værdi => _vertices = new List<Vertex>();
		/// </summary>
		private CityGraph()
		{
			_vertices = new List<Vertex>();
		}

		/// <summary>
		/// Properties til CityGraph//
		/// Her hentes instansen der bruges til alle objekter i klassen GraphCity();
		/// </summary>
		public static CityGraph Instance
		{
			get
			{
				if (_instance == null)
					_instance = new CityGraph();
				return _instance;
			}
		}

		/// <summary>
		/// Property til string [] array af Cities
		/// Her hentes alle Citynames fra listen 
		/// </summary>
		
		public string[] Cities
		{
			get { return _vertices.Select(v => v.CityName).ToArray(); }
		}
		/// <summary>
		/// Metode InsertVertex indsætter alle de vertices i deres poistioner;
		/// InserVertex tager parameter params string[] citynames til specificering af positioner til de indsatte vertices i graphen
		/// </summary>
		/// <param name="cityNames"></param>
		public void InsertVertex(params string[] cityNames)
		{
			Debug.WriteLine("CityGraph.InsertVertex");

			Array.ForEach(cityNames,
				cityName => 
				{
					Debug.WriteLine(" -> Inserting vertex: {0}",
						args: cityName);

					var temp = new Vertex(cityName);
					AddEdges(temp);
					_vertices.Add(temp);
				});

			Debug.WriteLine("");
		}
		/// <summary>
		/// Method AddEdges add edges between vertices if edges exist 
		/// Method takes vertex as a parameter.
		/// vertex consist of a single instance of cityname and usually will be connected with other vertices when edges are added to graph;
		/// Between 2 or more vertices there is a price which are also added as soon as the edges are added. 
		/// </summary> 
		/// <param name="vertex"></param>
		private void AddEdges(Vertex vertex)
		{
			Debug.WriteLine("CityGraph.AddEdges");

			foreach (var temp in _vertices)
			{
				decimal price = _randomizer.Next(300, 1000);

				if (!vertex.Edges.Exists(e => e.Endpoint.Equals(temp)))
				{
					Debug.WriteLine(" -> Adding edge from: {0} to: {1} with price: {2}",
						vertex.CityName,
						temp.CityName,
						price);
					vertex.Edges.Add(new Edge(temp, price));
				}

				if (!temp.Edges.Exists(e => e.Endpoint.Equals(vertex)))
				{
					Debug.WriteLine(" -> Adding edge from: {0} to: {1} with price: {2}",
						temp.CityName,
						vertex.CityName,
						price);
					temp.Edges.Add(new Edge(vertex, price));
				}
			}

			Debug.WriteLine("");
		}
		/// <summary>
		/// Method RemoveEdges() remove a vertex from graph from its current position:
		/// </summary>
		/// <param name="waypointA"></param>
		/// <param name="waypointB"></param>
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
		/// <summary>
		/// ListDestinations show all the list of the Citynames destination from a specific position
		/// ListDestination method takes a parameter string initial som refererer til startposition;
		/// Startposition(initial) referes to specific cityname destinations and will be listed as the list are not equal to the initial vetex
		
		/// </summary>
		/// <param name="initial"></param>
		/// <returns></returns>

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

			if (initial != lastInitial)
			{
				Debug.WriteLine("CityGraph.FindCheapestPath");

				var priorityQueue = new List<Vertex>();

				Debug.WriteLine(" -- Running cleanup");

				foreach (var vertex in _vertices)
				{
					vertex.Previous = null;
					vertex.PriceFromInitialVertex = 0;
					vertex.Visited = false;
				}

				var initialVertex = FindVertex(initial);

				Debug.WriteLine(" -> Enqueing: {0}"
					+ "\n    Price from initial vertex: {1}"
					+ "\n    Queue size: {2}",
					initialVertex.CityName,
					initialVertex.PriceFromInitialVertex,
					priorityQueue.Count);

				priorityQueue.Add(initialVertex);
				var frontVertex = initialVertex;
				var verticesVisited = 0;

				while (priorityQueue.Count > 0 && verticesVisited < _vertices.Count)
				{
					decimal value = decimal.MaxValue;

					foreach (var v in priorityQueue)
					{
						if (v.PriceFromInitialVertex < value)
						{
							frontVertex = v;
							value = v.PriceFromInitialVertex;
						}
					}

					frontVertex.Visited = true;
					verticesVisited++;

					Debug.WriteLine(" <- Dequeing: {0} {{weight: {1}}}"
						+ "\n    Queue size: {2}",
						frontVertex.CityName,
						frontVertex.PriceFromInitialVertex,
						priorityQueue.Count);

					priorityQueue.Remove(frontVertex);

					foreach (var e in frontVertex.Edges)
					{
						var price = e.Price;

						if (!e.Endpoint.Visited)
						{
							var found = false;
							var priceIsLess = false;

							foreach (var v in priorityQueue)
							{
								if (v.CityName.Equals(e.Endpoint.CityName, StringComparison.InvariantCultureIgnoreCase))
								{
									found = true;

									if (frontVertex.PriceFromInitialVertex + price < v.PriceFromInitialVertex)
										priceIsLess = true;
									else
										priceIsLess = false;
								}
							}

							if (found && priceIsLess)
							{
								var tmp = e.Endpoint;

								Debug.WriteLine(" -- Overwriting: {0}"
									+ "\n    Price from initial vertex: {1}",
									e.Endpoint.CityName,
									e.Endpoint.PriceFromInitialVertex);

								tmp.PriceFromInitialVertex = frontVertex.PriceFromInitialVertex + price;
								tmp.Previous = frontVertex;
							}
							else if (!found)
							{
								e.Endpoint.PriceFromInitialVertex = frontVertex.PriceFromInitialVertex + price;
								e.Endpoint.Previous = frontVertex;

								Debug.WriteLine(" -> Enqueing: {0}"
									+ "\n    Previous vertex: {1}"
									+ "\n    Price from initial vertex: {2}"
									+ "\n    Queue size: {3}",
									e.Endpoint.CityName,
									e.Endpoint.Previous.CityName,
									e.Endpoint.PriceFromInitialVertex,
									priorityQueue.Count);

								priorityQueue.Add(e.Endpoint);
							}
						}
					}
				}
			}
			else
				Debug.WriteLine(" -- Reusing previous calculation");

			lastInitial = initial;

			var temp = FindVertex(destination);
			var totalPrice = temp.PriceFromInitialVertex;

			while (temp != null)
			{
				Debug.WriteLine(" -- Backtracing: {0}",
					args: temp.CityName);

				resultingPath.Add(temp);
				temp = temp.Previous;
			}

			string[] waypoints = null;

			if (resultingPath.Count > 0)
			{
				waypoints = resultingPath.Select(v => v.CityName).Reverse().ToArray();
				Debug.WriteLine(" -- Resulting path of cost {0}:\n    {1}\n",
					totalPrice,
					string.Join("\n    ", waypoints));
			}
			else
				Debug.WriteLine(" !! No path found\n");

			return new Tuple<string[], decimal>(waypoints, totalPrice);
		}

		public void ApplyDiscount(string waypointA, string waypointB)
		{
			Debug.WriteLine("CityGraph.ApplyDiscount");

			decimal price = _randomizer.Next(100, 200);

			var vertexA = FindVertex(waypointA);
			var vertexB = FindVertex(waypointB);

			Debug.WriteLine(" -- Overwriting edge between: {0} and: {1}"
				+ "New Price: {2}",
				vertexA.CityName,
				vertexB.CityName,
				price);
			vertexA.Edges.Single(e => e.Endpoint.Equals(vertexB)).Price = price;

			Debug.WriteLine(" -- Overwriting edge between: {0} and: {1}"
				+ "New Price: {2}",
				vertexB.CityName,
				vertexA.CityName,
				price);
			vertexB.Edges.Single(e => e.Endpoint.Equals(vertexA)).Price = price;

			Debug.WriteLine("");
		}

		private Vertex FindVertex(string cityName)
		{
			return _vertices
				.Find(v => v.CityName.Equals(cityName, StringComparison.InvariantCultureIgnoreCase));
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
