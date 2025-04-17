using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class StageView : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller enhancedScroller;
    [SerializeField] EnhancedScrollerCellView rowViewPrefab;
    [SerializeField] private GameModel gameModel;

    private void Awake()
    {
        enhancedScroller.Delegate = this;
    }

    private void OnEnable()
    {
        gameModel.OnLoadStageDone += ShowStages;
    }

    private void OnDisable()
    {
        gameModel.OnLoadStageDone -= ShowStages;
    }

    private void ShowStages()
    {
        enhancedScroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return gameModel.NumberOfStages / StageRowView.NUMBER_OF_STAGES_PER_ROW;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 250f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var rowView = scroller.GetCellView(rowViewPrefab) as StageRowView;
        if (!rowView) return null;
        var startingIndex = dataIndex * StageRowView.NUMBER_OF_STAGES_PER_ROW;
        rowView.name = "Stage " + startingIndex + " to " + (startingIndex + StageRowView.NUMBER_OF_STAGES_PER_ROW - 1);
        rowView.SetData(gameModel.Stages, startingIndex);
        return rowView;
    }
}