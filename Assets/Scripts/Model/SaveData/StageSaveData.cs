using System;

[Serializable]
public class StageSaveData : ISaveAndLoad
{
    public int Stage { get; set; }
    public int Star { get; set; }
    public bool IsUnlocked { get; set; }

    public void Save()
    {
        LocalStorageUtils.SaveStage(this);
    }

    public void Load()
    {
        if (!LocalStorageUtils.HasKey($"{LocalStorageEnum.StageData}{Stage}")) return;
        var data = LocalStorageUtils.LoadStage(Stage);
        Star = data.Star;
        IsUnlocked = data.IsUnlocked;
    }
}