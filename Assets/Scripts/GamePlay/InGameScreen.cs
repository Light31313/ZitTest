using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreen : UIScreen
{
    [SerializeField] private Button btnMenu, btnShowPath, btnAutoMove;
    [SerializeField] private GameModel gameModel;
    [SerializeField] private TextMeshProUGUI txtStage;
    [SerializeField] private GamePlayController gamePlayController;

    private void Start()
    {
        btnMenu.onClick.AddListener(gameModel.BackToStages);
        btnShowPath.onClick.AddListener(gamePlayController.GeneratePath);
        btnAutoMove.onClick.AddListener(gamePlayController.MoveToDestination);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateStage();
        gameModel.OnReachNewStage += UpdateStage;
    }

    private void OnDisable()
    {
        gameModel.OnReachNewStage -= UpdateStage;
    }

    private void UpdateStage()
    {
        txtStage.text = gameModel.CurrentStage == 1 ? "TUTORIAL" : gameModel.CurrentStage.ToString();
    }
}