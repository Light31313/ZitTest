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
        btnShowPath.onClick.AddListener(() => { });
        btnAutoMove.onClick.AddListener(() => { });
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void OnDisable()
    {
    }

    private void UpdateStage()
    {
        txtStage.text = gameModel.CurrentStage == 1 ? "TUTORIAL" : gameModel.CurrentStage.ToString();
    }
}