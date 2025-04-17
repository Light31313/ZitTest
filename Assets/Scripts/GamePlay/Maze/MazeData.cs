using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "MazeData", menuName = "Data/MazeData", order = 1)]
public class MazeData : ScriptableObject
{
    [SerializeField] private Vector2Int mazeSize = new(10, 13);
    public Vector2Int MazeSize => mazeSize;
    [SerializeField] private int numberOfMazes = 999;
    public int NumberOfMazes => numberOfMazes;
    [SerializeField] private List<MazeItemData> mazeItems;
    public IReadOnlyList<MazeItemData> MazeItems => mazeItems;

    [Button]
    public void GenerateRandomMazes()
    {
        mazeItems = new List<MazeItemData>();
        for (var i = 0; i < numberOfMazes; i++)
        {
            mazeItems.Add(new MazeItemData(mazeSize));
        }
    }
}

[Serializable]
public class MazeItemData
{
    [SerializeField] private List<MazeCellItemData> cellItems;
    public List<MazeCellItemData> CellItems => cellItems;
    private Dictionary<Vector2Int, MazeCellItemData> _mazeDictionary;

    public Dictionary<Vector2Int, MazeCellItemData> MazeDictionary
    {
        get { return _mazeDictionary ??= cellItems.ToDictionary(item => item.Position); }
    }

    private Vector2Int _mazeSize;

    public MazeItemData(Vector2Int mazeSize)
    {
        _mazeSize = mazeSize;
        cellItems = new List<MazeCellItemData>();
        for (var i = 0; i < mazeSize.x; i++)
        {
            for (var j = 0; j < mazeSize.y; j++)
            {
                var pos = new Vector2Int(i, j);
                var mazeItem = new MazeCellItemData(pos);
                cellItems.Add(mazeItem);
            }
        }

        GenerateMaze(null, MazeDictionary[new Vector2Int(0, _mazeSize.y - 1)]);
    }

    private void GenerateMaze(MazeCellItemData previousCell, MazeCellItemData currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCellItemData nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCellItemData GetNextUnvisitedCell(MazeCellItemData currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        if (unvisitedCells.Count == 0) return null;
        return unvisitedCells.GetRandom();
    }

    private List<MazeCellItemData> GetUnvisitedCells(MazeCellItemData currentCell)
    {
        var x = (int)currentCell.Position.x;
        var y = (int)currentCell.Position.y;
        var unVisitedCells = new List<MazeCellItemData>();

        if (x + 1 < _mazeSize.x)
        {
            var cellToRight = MazeDictionary[new Vector2Int(x + 1, y)];

            if (cellToRight.IsVisited == false)
            {
                unVisitedCells.Add(cellToRight);
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = MazeDictionary[new Vector2Int(x - 1, y)];

            if (cellToLeft.IsVisited == false)
            {
                unVisitedCells.Add(cellToLeft);
            }
        }

        if (y + 1 < _mazeSize.y)
        {
            var cellToDown = MazeDictionary[new Vector2Int(x, y + 1)];

            if (cellToDown.IsVisited == false)
            {
                unVisitedCells.Add(cellToDown);
            }
        }

        if (y - 1 >= 0)
        {
            var cellToUp = MazeDictionary[new Vector2Int(x, y - 1)];

            if (cellToUp.IsVisited == false)
            {
                unVisitedCells.Add(cellToUp);
            }
        }

        return unVisitedCells;
    }

    private void ClearWalls(MazeCellItemData previousCell, MazeCellItemData currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.Position.x < currentCell.Position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.Position.x > currentCell.Position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.Position.y < currentCell.Position.y)
        {
            previousCell.ClearUpWall();
            currentCell.ClearDownWall();
            return;
        }

        if (previousCell.Position.y > currentCell.Position.y)
        {
            previousCell.ClearDownWall();
            currentCell.ClearUpWall();
        }
    }
}

[Serializable]
public class MazeCellItemData
{
    [SerializeField] private Vector2Int position;
    public Vector2Int Position => position;

    [SerializeField] private bool hasLeftWall = true, hasRightWall = true, hasDownWall = true, hasUpWall = true;
    public bool HasLeftWall => hasLeftWall;
    public bool HasRightWall => hasRightWall;
    public bool HasDownWall => hasDownWall;

    public bool HasUpWall => hasUpWall;

    //Use only to generate maze
    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
    }

    public MazeCellItemData(Vector2Int position)
    {
        this.position = position;
    }

    public void ClearLeftWall()
    {
        hasLeftWall = false;
    }

    public void ClearRightWall()
    {
        hasRightWall = false;
    }

    public void ClearUpWall()
    {
        hasUpWall = false;
    }

    public void ClearDownWall()
    {
        hasDownWall = false;
    }
}