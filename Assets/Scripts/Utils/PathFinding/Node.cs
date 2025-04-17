using UnityEngine;

public class Node
{
    public readonly int x, y;
    public readonly int g, h; // Cost components
    public int F => g + h; // Total cost
    public readonly Node fromNode;

    public Node(Vector2Int pos, int g, int h, Node fromNode)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.g = g;
        this.h = h;
        this.fromNode = fromNode;
    }

    public override string ToString()
    {
        return $"Node Info with g: {g}, h: {h}, x: {x}, y: {y}";
    }
}


/*// Example usage
class Program
{
    static void Main()
    {
        int[,] grid = {
            { 0, 0, 1, 0, 0 },
            { 0, 1, 1, 0, 1 },
            { 0, 0, 0, 0, 0 },
            { 1, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0 }
        };

        var start = new Node(0, 0, 0, 0, null);
        var goal = new Node(4, 4, 0, 0, null);

        var path = AStarPathfinding.FindPath(grid, start, goal);
        if (path != null)
        {
            Console.WriteLine("Path found!");
            AStarPathfinding.PrintGrid(grid, path);
        }
        else
        {
            Console.WriteLine("No path available.");
        }
    }
}*/