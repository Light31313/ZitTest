using System.Collections;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Destination destination;
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private GameModel gameModel;
    [SerializeField] private Vector2Int playerSpawnCell = new(0, 12);
    [SerializeField] private MazeData mazeData;

    private void OnEnable()
    {
        InitGamePlay();
        gameModel.OnReachNewStage += InitGamePlay;
    }

    private void OnDisable()
    {
        gameModel.OnReachNewStage -= InitGamePlay;
    }

    private void InitGamePlay()
    {
        mazeGenerator.GenerateMaze();
        StartCoroutine(IEWaitInit());

        IEnumerator IEWaitInit()
        {
            yield return null;
            player.CurrentCell = playerSpawnCell;
            player.transform.position = mazeGenerator.CellViewsDictionary[playerSpawnCell].transform.position;
            var randomDesCell = GetRandomDestinationCell();
            destination.CurrentCell = randomDesCell;
            destination.transform.position = mazeGenerator.CellViewsDictionary[randomDesCell].transform.position;
        }
    }

    public void GeneratePath()
    {
        mazeGenerator.FindPath(player.CurrentCell, destination.CurrentCell);
    }

    public void MoveToDestination()
    {
        
    }

    private Vector2Int GetRandomDestinationCell()
    {
        var randomDesCell = new Vector2Int(Random.Range(0, mazeData.MazeSize.x), Random.Range(0, mazeData.MazeSize.y));
        if (randomDesCell == playerSpawnCell) randomDesCell = GetRandomDestinationCell();
        return randomDesCell;
    }
}