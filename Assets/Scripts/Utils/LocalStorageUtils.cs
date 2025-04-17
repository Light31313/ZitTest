using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public static class LocalStorageUtils
{
    public static PlayerSaveData PlayerSaveData
    {
        get => GetObject<PlayerSaveData>(LocalStorageEnum.PlayerData.ToString());
        set => SetObject(LocalStorageEnum.PlayerData.ToString(), value);
    }

    public static bool HasInitStages
    {
        get => GetBoolean(LocalStorageEnum.InitStages.ToString());
        set => SetObject(LocalStorageEnum.InitStages.ToString(), value);
    }

    public static void SaveStage(StageSaveData data)
    {
        SetObject($"{LocalStorageEnum.StageData}{data.Stage}", data);
    }

    public static StageSaveData LoadStage(int stage)
    {
        return GetObject<StageSaveData>($"{LocalStorageEnum.StageData}{stage}");
    }

    public static string GetString(string key)
    {
        var data = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(data)) return "";
        return EncryptUtils.Decrypt(data);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, EncryptUtils.Encrypt(value));
    }

    public static bool GetBoolean(string key, bool defaultValue = false)
    {
        var value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        if (value == 1) return true;
        return false;
    }

    public static void SetBoolean(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static void SetLong(string key, long value)
    {
        PlayerPrefs.SetString(key, EncryptUtils.Encrypt(value.ToString()));
    }

    public static long GetLong(string key, long d = 0)
    {
        var value = PlayerPrefs.GetString(key);
        if (!string.IsNullOrEmpty(value))
        {
            return long.Parse(EncryptUtils.Decrypt(value));
        }

        return d;
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static List<T> GetList<T>(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            var d = JsonConvert.DeserializeObject(PlayerPrefs.GetString(key),
                typeof(List<T>));
            return d as List<T>;
        }

        return new List<T>();
    }

    public static void SetList<T>(string key, List<T> values)
    {
        var jsonValues = JsonConvert.SerializeObject(values);
        PlayerPrefs.SetString(key, jsonValues);
    }

    public static T GetObject<T>(string key) where T : new()
    {
        if (PlayerPrefs.HasKey(key))
        {
            var d = (T)JsonConvert.DeserializeObject(PlayerPrefs.GetString(key), typeof(T));
            return d;
        }

        return new T();
    }

    public static void SetObject<T>(string key, T value)
    {
        var jsonValue = JsonConvert.SerializeObject(value);
        PlayerPrefs.SetString(key, jsonValue);
    }
}

public enum LocalStorageEnum
{
    PlayerData,
    StageData,
    InitStages
}