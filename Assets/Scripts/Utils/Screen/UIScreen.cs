using System;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] private ScreenId screenId;
    [SerializeField] private float openDuration = 0.1f;
    [SerializeField] private float closeDuration = 0.1f;
    public float OpenDuration => openDuration;
    public float CloseDuration => closeDuration;

    public ScreenId ScreenId => screenId;

    protected virtual void OnEnable()
    {
        ScreenManager.Instance.CurrentOpenScreen = this;
    }
}

public enum ScreenId
{
    InGame,
    Stage
}