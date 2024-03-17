using DijkstraConsoleApp;

[TestClass]
public class GraphTest
{
    [TestMethod]
    public void ShortestPath_ShouldFindCorrectPath_WhenPathExists()
    {
        // Arrange
        var graph = new Graph();
        graph.AddNode("A");
        graph.AddNode("B");
        graph.AddNode("C");
        graph.AddEdge("A", "B", 1);
        graph.AddEdge("B", "C", 2);
        graph.AddEdge("A", "C", 4);

        // Act
        var result = graph.ShortestPath("A", "C");

        // Assert
        Assert.AreEqual(3, result.Distance);
        CollectionAssert.AreEqual(new List<string> { "A", "B", "C" }, result.NodeNames);
    }

    [TestMethod]
    public void ShortestPath_ShouldReturnNegativeDistance_WhenNoPathExists()
    {
        // Arrange
        var graph = new Graph();
        graph.AddNode("A");
        graph.AddNode("B");

        // Act
        var result = graph.ShortestPath("A", "B");

        // Assert
        Assert.AreEqual(-1, result.Distance);
    }
}
