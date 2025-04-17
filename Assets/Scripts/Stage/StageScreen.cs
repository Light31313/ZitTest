using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageScreen : UIScreen
{
    [SerializeField] private Button btnReset;
    [SerializeField] private TextMeshProUGUI txtTotalStar;
    [SerializeField] private GameModel gameModel;

    private void Start()
    {
        btnReset.onClick.AddListener(gameModel.ResetStages);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        gameModel.OnLoadStageDone += UpdateTotalStars;
    }

    private void OnDisable()
    {
        gameModel.OnLoadStageDone -= UpdateTotalStars;
    }

    private void UpdateTotalStars()
    {
        txtTotalStar.text = gameModel.TotalStars.ToString();
    }
}