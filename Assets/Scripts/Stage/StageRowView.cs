using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class StageRowView : EnhancedScrollerCellView
{
    [SerializeField] private StageItemView[] stagesItemView;
    public const int NUMBER_OF_STAGES_PER_ROW = 4;

    public void SetData(IReadOnlyList<StageSaveData> stageItems, int startingIndex)
    {
        for (var i = 0; i < stagesItemView.Length; i++)
        {
            var index = startingIndex % 8 is >= 4 and <= 7 ? stagesItemView.Length - 1 - i : i;
            if (startingIndex + i < stageItems.Count && i < NUMBER_OF_STAGES_PER_ROW)
            {
                stagesItemView[index].Show();
                stagesItemView[index]
                    .Init(stageItems[startingIndex + i]);
            }
            else
            {
                stagesItemView[i].Hide();
            }
        }
    }
}