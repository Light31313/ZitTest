using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCellView cellViewPrefab;
    [SerializeField] private MazeData mazeData;
    [SerializeField] private Line linePrefab;
    [SerializeField] private GameModel gameModel;
    public MazeItemData CurrentMazeItem => mazeData.MazeItems[gameModel.CurrentStage - 1];
    private readonly Dictionary<Vector2Int, MazeCellView> _cellViewsDictionary = new();
    public IReadOnlyDictionary<Vector2Int, MazeCellView> CellViewsDictionary => _cellViewsDictionary;
    private Transform _cacheTransform;
    private Dictionary<Vector2Int, bool> _currentGrid;
    private List<Line> _guidingLines = new();
    private List<Vector2> _pathPoints = new();

    private void Awake()
    {
        _cacheTransform = transform;
    }

    private void OnDisable()
    {
        ClearMaze();
        ClearLine();
    }

    public void GenerateMaze()
    {
        ClearMaze();
        ClearLine();
        var mazeSize = mazeData.MazeSize;
        for (var i = mazeSize.y - 1; i >= 0; i--)
        {
            for (var j = 0; j < mazeSize.x; j++)
            {
                var pos = new Vector2Int(j, i);
                var mazeItem = CurrentMazeItem.MazeDictionary[pos];
                var cellView = Pool.Get(cellViewPrefab, _cacheTransform);
                cellView.Init(mazeItem);
                _cellViewsDictionary.Add(pos, cellView);
            }
        }

        _currentGrid = GetGridOfCurrentStage();
    }

    private void ClearMaze()
    {
        foreach (var item in _cellViewsDictionary.Values)
        {
            Pool.Release(item);
        }

        _cellViewsDictionary.Clear();
    }

    private void ClearLine()
    {
        foreach (var item in _guidingLines)
        {
            Pool.Release(item);
        }

        _guidingLines.Clear();
    }

    public void FindPath(Vector2Int startCell, Vector2Int endCell)
    {
        var path = AStarPathfinding.FindPath(_currentGrid,
            new Node(CellToGridPos(startCell), 0, 0, null),
            new Node(CellToGridPos(endCell), 0, 0, null));
        if (path.Count == 0) return;
        var startLinePos = new Vector2Int(path[0].x, path[0].y);
        var endLinePos = Vector2Int.zero;
        for (var i = 1; i < path.Count; i++)
        {
            var node = path[i];
            var nodePos = new Vector2Int(node.x, node.y);
            if (startLinePos.x == nodePos.x || startLinePos.y == nodePos.y)
            {
                endLinePos = nodePos;
            }

            if ((startLinePos.x != nodePos.x && startLinePos.y != nodePos.y) || i == path.Count - 1)
            {
                var line = Pool.Get(linePrefab, transform.parent);
                line.InitLine(GridPosToCellPos(startLinePos),
                    GridPosToCellPos(endLinePos));
                startLinePos = endLinePos;
                endLinePos = nodePos;
            }
        }
    }

    // Generate Grid for A* PathFinding
    private Dictionary<Vector2Int, bool> GetGridOfCurrentStage()
    {
        var grid = new Dictionary<Vector2Int, bool>();
        foreach (var item in CurrentMazeItem.CellItems)
        {
            var basePos = CellToGridPos(item.Position);
            grid[basePos] = true;
            grid[basePos + Vector2Int.left] = !item.HasLeftWall;
            grid[basePos + Vector2Int.right] = !item.HasRightWall;
            grid[basePos + Vector2Int.up] = !item.HasUpWall;
            grid[basePos + Vector2Int.down] = !item.HasDownWall;
        }

        return grid;
    }

    public Vector2Int CellToGridPos(Vector2Int cellPos)
    {
        return new Vector2Int(cellPos.x * 2 + 1, cellPos.y * 2 + 1);
    }

    private Vector2 GridPosToCellPos(Vector2Int gridPos)
    {
        return ((RectTransform)_cellViewsDictionary[new Vector2Int((gridPos.x - 1) / 2, (gridPos.y - 1) / 2)].transform)
            .anchoredPosition;
    }
}