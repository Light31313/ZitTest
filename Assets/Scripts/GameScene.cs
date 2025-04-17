using UnityEngine;

public class GameScene : UIScene
{
    [SerializeField] private GameModel gameModel;


    private void Start()
    {
        gameModel.LoadGameData();
    }

    private void OnApplicationQuit()
    {
        gameModel.SavePlayerData();
    }
}