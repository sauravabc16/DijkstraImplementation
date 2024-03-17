using System;
namespace DijkstraConsoleApp
{
    public class Node
    {
        public string Name { get; set; }
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int Distance { get; set; } = int.MaxValue;
        public Node Predecessor { get; set; } = null;
    }
}

