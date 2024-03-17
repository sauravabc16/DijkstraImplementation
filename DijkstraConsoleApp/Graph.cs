namespace DijkstraConsoleApp
{
    public class Graph
    {
        // Dictionary to hold nodes. Key is the node name, value is the Node object.
        public Dictionary<string, Node> Nodes { get; set; } = new Dictionary<string, Node>();

        /// <summary>
        /// Adds a new node to the graph.
        /// </summary>
        /// <param name="name">The name of the node to add.</param>
        public void AddNode(string name)
        {
            // Create a new node and add it to the dictionary
            Nodes[name] = new Node { Name = name };
        }

        /// <summary>
        /// Adds an edge between two nodes in the graph.
        /// </summary>
        /// <param name="from">The starting node of the edge.</param>
        /// <param name="to">The ending node of the edge.</param>
        /// <param name="weight">The weight of the edge.</param>
        /// <param name="bidirectional">Flag indicating if the edge is bidirectional.</param>
        public void AddEdge(string from, string to, int weight, bool bidirectional = false)
        {
            if (!Nodes.ContainsKey(from) || !Nodes.ContainsKey(to))
                throw new ArgumentException("Both nodes must exist.");

            // Add edge from 'from' node to 'to' node
            Nodes[from].Edges.Add(new Edge { Target = Nodes[to], Weight = weight });

            // If the edge is bidirectional, add an edge in the opposite direction
            if (bidirectional)
            {
                Nodes[to].Edges.Add(new Edge { Target = Nodes[from], Weight = weight });
            }
        }

        /// <summary>
        /// Finds the shortest path between two nodes using Dijkstra's algorithm.
        /// </summary>
        /// <param name="startName">The starting node's name.</param>
        /// <param name="endName">The ending node's name.</param>
        /// <returns>A ShortestPathData object containing the path and its total distance.</returns>
        public ShortestPathData ShortestPath(string startName, string endName)
        {
            // Check if both start and end nodes exist in the graph. If not, throw an exception.
            if (!Nodes.ContainsKey(startName) || !Nodes.ContainsKey(endName))
                throw new ArgumentException("Both nodes must exist in the graph.");

            // Initialize all nodes' distances to infinity and predecessors to null.
            // This is a standard step in Dijkstra's algorithm to prepare for distance calculations.
            foreach (var node in Nodes.Values)
            {
                node.Distance = int.MaxValue;
                node.Predecessor = null;
            }

            // Set the distance of the start node to 0 since it's the starting point.
            var startNode = Nodes[startName];
            startNode.Distance = 0;

            // Use a priority queue to determine the next node to visit based on the shortest distance.
            // This is a key component of Dijkstra's algorithm, ensuring the shortest paths are calculated efficiently.
            var priorityQueue = new PriorityQueue<Node, int>();
            var visited = new HashSet<Node>(); // Keeps track of visited nodes to avoid reprocessing.

            // Add the start node to the queue with a priority of 0.
            priorityQueue.Enqueue(startNode, startNode.Distance);

            // Continue the loop until there are no more nodes to process in the queue.
            while (priorityQueue.Count > 0)
            {
                // Dequeue the node with the shortest distance from the start node.
                Node current = priorityQueue.Dequeue();
                visited.Add(current); // Mark the current node as visited.

                // If the current node is the destination, we can break out of the loop early.
                if (current.Name == endName)
                    break;

                // Iterate through each edge of the current node to explore its neighbors.
                foreach (var edge in current.Edges)
                {
                    // Skip any neighbor nodes that have already been visited.
                    if (visited.Contains(edge.Target))
                        continue;

                    // Calculate the distance from the start node to this neighbor node via the current node.
                    int distance = current.Distance + edge.Weight;

                    // If this newly calculated distance is shorter than the previously recorded distance,
                    // update the neighbor node's distance and predecessor.
                    if (distance < edge.Target.Distance)
                    {
                        edge.Target.Distance = distance;
                        edge.Target.Predecessor = current;
                        // Re-add the neighbor node to the priority queue with its updated priority (distance).
                        priorityQueue.Enqueue(edge.Target, distance);
                    }
                }
            }

            // Once the shortest path to the destination has been determined (or the queue is empty),
            // backtrack from the destination node to the start node to construct the shortest path.
            var path = new List<string>();
            var currentNode = Nodes[endName];

            // If the distance to the end node is still infinity, no path exists.
            if (currentNode.Distance == int.MaxValue)
                return new ShortestPathData { NodeNames = new List<string>(), Distance = -1 };

            // Backtrack from the end node to the start node using the predecessor nodes.
            while (currentNode != null)
            {
                path.Insert(0, currentNode.Name);
                currentNode = currentNode.Predecessor;
            }

            // Return the shortest path data, including the path itself and its total distance.
            return new ShortestPathData { NodeNames = path, Distance = Nodes[endName].Distance };
        }

        /// <summary>
        /// Displays the graph in a readable format.
        /// </summary>
        public void DisplayGraph()
        {
            Console.WriteLine("Graph Visualization:");
            foreach (var node in Nodes)
            {
                var edges = node.Value.Edges
                    .Select(edge => $"{edge.Target.Name}({edge.Weight})")
                    .ToList();

                string edgesText = string.Join(", ", edges);
                Console.WriteLine($"Node {node.Key} -> {edgesText}");
            }
        }

        /// <summary>
        /// Checks if an edge exists between two nodes.
        /// </summary>
        /// <param name="from">The starting node of the edge.</param>
        /// <param name="to">The ending node of the edge.</param>
        /// <returns>True if the edge exists, false otherwise.</returns>
        public bool HasEdge(string from, string to)
        {
            if (!Nodes.ContainsKey(from))
            {
                return false;
            }

            return Nodes[from].Edges.Any(e => e.Target.Name == to);
        }
    }
}

