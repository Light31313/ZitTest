using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "GameModel", menuName = "Model/GameModel", order = 1)]
public class GameModel : ScriptableObject
{
    [SerializeField] private MazeData mazeData;
    private List<StageSaveData> _stages;
    public IReadOnlyList<StageSaveData> Stages => _stages;
    private PlayerSaveData _player;
    public int TotalStars { get; private set; } = 0;
    public event Action OnLoadStageDone;
    public event Action<PlayerSaveData> OnLoadPlayerDone;
    public event Action OnReachNewStage;
    public int NumberOfStages => _stages.Count;
    public int CurrentStage { get; private set; }

    public void LoadGameData()
    {
        LoadStage();
        LoadPlayer();

        void LoadPlayer()
        {
            _player = new PlayerSaveData();
            _player.Load();
            OnLoadPlayerDone?.Invoke(_player);
        }

        void LoadStage()
        {
            if (!LocalStorageUtils.HasInitStages)
            {
                var numberOfStageUnlocked = Random.Range(1, 1000);
                for (var i = 1; i <= mazeData.NumberOfMazes; i++)
                {
                    var isUnlocked = i <= numberOfStageUnlocked;
                    var stage = new StageSaveData
                    {
                        Stage = i,
                        IsUnlocked = isUnlocked,
                        Star = isUnlocked ? Random.Range(1, 4) : 0
                    };
                    stage.Save();
                }

                LocalStorageUtils.HasInitStages = true;
            }

            UpdateStages();
        }
    }

    public void OpenStage(int stage)
    {
        CurrentStage = stage;
        ScreenManager.Instance.OpenScreen(ScreenId.InGame);
    }

    public void BackToStages()
    {
        ScreenManager.Instance.OpenScreen(ScreenId.Stage);
    }

    public void ReachNextStage()
    {
        CurrentStage++;
        OnReachNewStage?.Invoke();
    }

    private void UpdateStages()
    {
        _stages = new();
        TotalStars = 0;
        for (var i = 1; i <= mazeData.NumberOfMazes; i++)
        {
            var stage = new StageSaveData
            {
                Stage = i,
            };
            stage.Load();
            TotalStars += stage.Star;
            _stages.Add(stage);
        }

        OnLoadStageDone?.Invoke();
    }

    public void SavePlayerData()
    {
        _player.Save();
    }

    public void ResetStages()
    {
        for (var i = 1; i <= mazeData.NumberOfMazes; i++)
        {
            var stage = new StageSaveData
            {
                Stage = i,
                IsUnlocked = i == 1,
                Star = 0
            };
            stage.Save();
        }

        UpdateStages();
    }

    [Button]
    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void OnDisable()
    {
        _stages = null;
        _player = null;
        CurrentStage = 0;
    }
}