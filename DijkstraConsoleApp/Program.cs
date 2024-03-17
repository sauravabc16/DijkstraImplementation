

using DijkstraConsoleApp;

class Program
{
    /// <summary>
    /// The Main method is the entry point for the application.
    /// It allows the user to choose between using a default graph or inputting their own graph.
    /// After initializing the graph, it finds the shortest path between two nodes.
    /// </summary>
    static void Main()
    {
        var graph = new Graph();
        bool validModeSelected = false;

        // Loop until a valid mode is selected
        while (!validModeSelected)
        {
            Console.WriteLine("Select mode: (1) Use default graph, (2) Enter own graph");
            string mode = Console.ReadLine().Trim();

            switch (mode)
            {
                case "1":
                    InitializeDefaultGraph(graph);
                    validModeSelected = true;
                    break;
                case "2":
                    InputGraphData(graph);
                    validModeSelected = true;
                    break;
                default:
                    Console.WriteLine("Invalid mode selected. Please try again.");
                    break;
            }
        }

        graph.DisplayGraph();

        // Main loop to allow multiple pathfindings or exit
        while (true)
        {
            string source;
            string destination;
            bool validInput = false;

            // Loop to ensure valid source and destination nodes are entered
            while (!validInput)
            {
                Console.WriteLine("Enter source node:");
                source = Console.ReadLine().ToUpper();

                if (!graph.Nodes.ContainsKey(source))
                {
                    Console.WriteLine("Source node does not exist. Please enter a valid node.");
                    continue;
                }

                Console.WriteLine("Enter destination node:");
                destination = Console.ReadLine().ToUpper();

                if (!graph.Nodes.ContainsKey(destination))
                {
                    Console.WriteLine("Destination node does not exist. Please enter a valid node.");
                    continue;
                }

                validInput = true;

                // Calculate and display the shortest path
                var shortestPathData = graph.ShortestPath(source, destination);
                if (shortestPathData.Distance >= 0)
                {
                    Console.WriteLine($"Shortest path from {source} to {destination}: " +
                                      String.Join(", ", shortestPathData.NodeNames));
                    Console.WriteLine($"Total Distance: {shortestPathData.Distance}");
                }
                else
                {
                    Console.WriteLine($"There is no path from {source} to {destination}.");
                }
            }

            // Ask the user if they want to find another shortest path
            Console.WriteLine("Do you want to find another shortest path? (Y/N)");
            string answer = Console.ReadLine().Trim().ToLower();
            if (answer.ToUpper() != "Y")
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                break; // Exit the while loop and end the program
            }
        }
    }

    /// <summary>
    /// Initializes the graph with a predefined set of nodes and edges.
    /// </summary>
    /// <param name="graph">The graph to initialize.</param>
    static void InitializeDefaultGraph(Graph graph)
    {
        string[] nodeNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
        foreach (var name in nodeNames)
        {
            graph.AddNode(name);
        }

        // Add bidirectional edges
        graph.AddEdge("A", "B", 4, true);
        graph.AddEdge("A", "C", 6, true);
        graph.AddEdge("C", "D", 8, true);
        graph.AddEdge("D", "E", 4, true);
        graph.AddEdge("D", "G", 1, true);
        graph.AddEdge("E", "F", 3, true);
        graph.AddEdge("F", "B", 2, true);
        graph.AddEdge("F", "H", 6, true);
        graph.AddEdge("G", "H", 5, true);
        graph.AddEdge("G", "F", 4, true);
        graph.AddEdge("E", "I", 8, true);
        graph.AddEdge("G", "I", 5, true);
        graph.AddEdge("E", "B", 2); // Only from E to B, not the other way around

        Console.WriteLine("Default graph loaded.");
    }

    /// <summary>
    /// Allows the user to input their own graph data, including nodes and edges.
    /// </summary>
    /// <param name="graph">The graph to add the data to.</param>
    static void InputGraphData(Graph graph)
    {
        Console.WriteLine("Enter the number of nodes:");
        int nodeCount = int.Parse(Console.ReadLine().Trim());

        for (int i = 0; i < nodeCount; i++)
        {
            Console.WriteLine($"Enter node name #{i + 1}:");
            string nodeName = Console.ReadLine().Trim().ToUpper();
            graph.AddNode(nodeName);
        }

        Console.WriteLine("Enter the number of edges:");
        int edgeCount = int.Parse(Console.ReadLine().Trim());

        for (int i = 0; i < edgeCount; i++)
        {
            bool validEdgeEntered = false;
            while (!validEdgeEntered)
            {
                //TODO: Add ability to add bidirectional edges.
                Console.WriteLine($"Enter edge #{i + 1} in the format 'FromNode ToNode Weight':");
                string[] edgeData = Console.ReadLine().Trim().Split(' ');
                if (edgeData.Length == 3)
                {
                    string fromNode = edgeData[0].ToUpper();
                    string toNode = edgeData[1].ToUpper();
                    int weight = int.Parse(edgeData[2]);

                    if (!graph.HasEdge(fromNode, toNode))
                    {
                        graph.AddEdge(fromNode, toNode, weight);
                        validEdgeEntered = true;
                    }
                    else
                    {
                        Console.WriteLine("This edge already exists. Please enter a different edge.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid edge format. Please enter the edge data in the correct format.");
                }
            }
        }

        Console.WriteLine("Custom graph loaded.");
    }
}








