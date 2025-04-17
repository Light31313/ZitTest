using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ScreenManager : MonoSingleton<ScreenManager>
{
    public UIScene CurrentOpenScene { get; set; }
    public UIScreen CurrentOpenScreen { get; set; }
    public static Action<UIScene> OnCloseCurrentScene;

    public bool IsOpenScreen(ScreenId screenId) => CurrentOpenScreen && (screenId == CurrentOpenScreen.ScreenId);
    public bool IsOpenScene(SceneId sceneId) => CurrentOpenScene && (sceneId == CurrentOpenScene.SceneId);

    public void OpenScene(SceneId sceneId)
    {
        if (sceneId == CurrentOpenScene.SceneId) return;
        Instance.StartCoroutine(IEOpenScene());

        IEnumerator IEOpenScene()
        {
            yield return CurrentOpenScene.CloseScreen().WaitForCompletion();
            var loadSceneHandle = Addressables.LoadSceneAsync(sceneId.ToString());
            OnCloseCurrentScene?.Invoke(CurrentOpenScene);
            yield return loadSceneHandle;
            if (loadSceneHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Load Scene Error");
            }
        }
    }

    public void OpenScene(SceneId sceneId, ScreenId screenId, Action onOpenComplete = null, bool needTransition = true)
    {
        if (sceneId == CurrentOpenScene.SceneId) return;
        Instance.StartCoroutine(IEOpenScene());

        IEnumerator IEOpenScene()
        {
            yield return CurrentOpenScene.CloseScreen().WaitForCompletion();
            var loadSceneHandle = Addressables.LoadSceneAsync(sceneId.ToString());
            OnCloseCurrentScene?.Invoke(CurrentOpenScene);
            yield return loadSceneHandle;

            if (loadSceneHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Load Scene Error");
            }
            else if (loadSceneHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (CurrentOpenScreen)
                {
                    if (screenId != CurrentOpenScreen.ScreenId)
                    {
                        CurrentOpenScreen.Hide();
                        ShowNewScreen(screenId);
                        onOpenComplete?.Invoke();
                    }
                }
                else
                {
                    ShowNewScreen(screenId);
                    onOpenComplete?.Invoke();
                }
            }
        }
    }

    public void OpenScreen(ScreenId screenId, Action onOpenComplete = null, bool needTransition = true)
    {
        if (CurrentOpenScreen && screenId == CurrentOpenScreen.ScreenId) return;
        Instance.StartCoroutine(IEOpenScreen());

        IEnumerator IEOpenScreen()
        {
            yield return CurrentOpenScene.CloseScreen(needTransition, CurrentOpenScreen.CloseDuration)
                ?.WaitForCompletion();
            CurrentOpenScreen.Hide();
            ShowNewScreen(screenId);
            yield return CurrentOpenScene.OpenScreen(needTransition, true, CurrentOpenScreen.OpenDuration)
                ?.WaitForCompletion();
            onOpenComplete?.Invoke();
        }
    }

    private void ShowNewScreen(ScreenId screenId)
    {
        var newScreen = CurrentOpenScene.GetScreenById(screenId);
        newScreen.Show();
    }
}