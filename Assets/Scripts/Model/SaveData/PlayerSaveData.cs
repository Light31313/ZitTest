using System;
using UnityEngine;

[Serializable]
public class PlayerSaveData : ISaveAndLoad
{
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int Level { get; set; }

    public void Save()
    {
        LocalStorageUtils.PlayerSaveData = this;
    }

    public void Load()
    {
        if (!LocalStorageUtils.HasKey(LocalStorageEnum.PlayerData.ToString())) return;
        var data = LocalStorageUtils.PlayerSaveData;
        PositionX = data.PositionX;
        PositionY = data.PositionY;
        Level = data.Level;
    }
}