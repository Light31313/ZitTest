using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Destination destination;
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private GameModel gameModel;
    [SerializeField] private Vector2Int playerSpawnCell = new(0, 12);
    [SerializeField] private MazeData mazeData;
    [SerializeField] private HoldableButton btnMoveUp, btnMoveDown, btnMoveRight, btnMoveLeft;
    private Coroutine _playerMoveCoroutine;

    private void Start()
    {
        btnMoveUp.onHold += MoveUp;
        btnMoveDown.onHold += MoveDown;
        btnMoveRight.onHold += MoveRight;
        btnMoveLeft.onHold += MoveLeft;
    }

    private void MoveLeft()
    {
        Move(Vector2Int.left);
    }

    private void MoveRight()
    {
        Move(Vector2Int.right);
    }

    private void MoveDown()
    {
        Move(Vector2Int.down);
    }

    private void MoveUp()
    {
        Move(Vector2Int.up);
    }

    private void Move(Vector2Int moveDirection)
    {
        if (player.IsMoving) return;
        player.Rotate(moveDirection == Vector2Int.up ? 0 :
            moveDirection == Vector2Int.left ? 90 :
            moveDirection == Vector2Int.right ? -90 : 180);
        var key = player.CurrentCell + moveDirection;
        if (!mazeGenerator.CellViewsDictionary.ContainsKey(key)) return;
        var facingCell = mazeGenerator.CellViewsDictionary[key];
        if (moveDirection == Vector2Int.up)
        {
            if (facingCell.Data.HasDownWall) return;
        }
        else if (moveDirection == Vector2Int.right)
        {
            if (facingCell.Data.HasLeftWall) return;
        }
        else if (moveDirection == Vector2Int.left)
        {
            if (facingCell.Data.HasRightWall) return;
        }
        else if (moveDirection == Vector2Int.down)
        {
            if (facingCell.Data.HasUpWall) return;
        }
        else
        {
            return;
        }

        player.CurrentCell += moveDirection;
        _playerMoveCoroutine =
            StartCoroutine(player.IEMoveToDestination(mazeGenerator.CellPosToAnchoredPos(player.CurrentCell),
                () =>
                {
                    if (destination.CurrentCell == player.CurrentCell)
                        gameModel.ReachNextStage();
                }));
    }

    private void OnEnable()
    {
        InitGamePlay();
        gameModel.OnReachNewStage += InitGamePlay;
    }

    private void OnDisable()
    {
        gameModel.OnReachNewStage -= InitGamePlay;
        StopPlayerMoving();
    }

    private void InitGamePlay()
    {
        mazeGenerator.GenerateMaze();
        StartCoroutine(IEWaitInit());

        IEnumerator IEWaitInit()
        {
            yield return null;
            StopPlayerMoving();
            player.CurrentCell = playerSpawnCell;
            player.SetPosition(mazeGenerator.CellPosToAnchoredPos(playerSpawnCell));
            var randomDesCell = GetRandomDestinationCell();
            destination.CurrentCell = randomDesCell;
            ((RectTransform)destination.transform).anchoredPosition = mazeGenerator.CellPosToAnchoredPos(randomDesCell);
        }
    }

    public void GeneratePath()
    {
        mazeGenerator.FindPath(player.CurrentCell, destination.CurrentCell);
        mazeGenerator.DrawLines();
    }

    private void StopPlayerMoving()
    {
        if (_playerMoveCoroutine != null) StopCoroutine(_playerMoveCoroutine);
        player.Stop();
        player.IsMoving = false;
    }
    
    public void MoveToDestination()
    {
        if (player.IsMoving) return;
        mazeGenerator.FindPath(player.CurrentCell, destination.CurrentCell);
        _playerMoveCoroutine =
            StartCoroutine(player.IEMoveAlongPath(mazeGenerator.PathPoints, gameModel.ReachNextStage));
    }

    private Vector2Int GetRandomDestinationCell()
    {
        var randomDesCell = new Vector2Int(Random.Range(0, mazeData.MazeSize.x), Random.Range(0, mazeData.MazeSize.y));
        if (randomDesCell == playerSpawnCell) randomDesCell = GetRandomDestinationCell();
        return randomDesCell;
    }
}