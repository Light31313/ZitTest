using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStarPathfinding
{
    private static readonly List<Vector2Int> Directions = new List<Vector2Int>()
        { new(0, 1), new(1, 0), new(-1, 0), new(0, -1) };

    public static List<Node> FindPath(IReadOnlyList<Vector2Int> grid, Node start, Node goal)
    {
        var openSet = new List<Node> { start };
        var closedSet = new HashSet<(int, int)>();

        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => a.F.CompareTo(b.F)); // Sort by lowest F cost
            var current = openSet[0];
            openSet.RemoveAt(0);
            closedSet.Add((current.x, current.y));

            if (current.x == goal.x && current.y == goal.y) return RetracePath(current);

            foreach (var dir in Directions)
            {
                var newX = current.x + dir.x;
                var newY = current.y + dir.y;
                var key = new Vector2Int(newX, newY);
                if (!grid.Contains(key)) continue; // Skip obstacles

                if (closedSet.Contains((newX, newY))) continue;

                var g = current.g + 1;
                var h = Math.Abs(newX - goal.x) + Math.Abs(newY - goal.y); // Manhattan heuristic
                var neighbor = new Node(new Vector2Int(newX, newY), g, h, current);

                if (openSet.Exists(n => n.x == newX && n.y == newY && g >= n.g)) continue;

                openSet.Add(neighbor);
            }
        }

        return null; // No path found
    }

    private static List<Node> RetracePath(Node node)
    {
        var path = new List<Node>();
        while (node != null)
        {
            path.Add(node);
            node = node.fromNode;
        }

        path.Reverse();
        return path;
    }
}