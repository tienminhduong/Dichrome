using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] private Image loadingCover;
    [SerializeField] private float fadeDuration = 0.5f;

    public static event Action<string> OnSceneLoadEnded;
    public static event Action<string> OnSceneUnloadStarted;

    private SceneInstance activeSceneInstance;


    public void LoadAddessableScene(string sceneAddress)
    {
        LoadAddressableSceneAsync(sceneAddress).Forget();
    }

    private async UniTask LoadAddressableSceneAsync(string sceneAddress)
    {
        await loadingCover.DOFade(1, fadeDuration).AsyncWaitForCompletion();
        loadingCover.raycastTarget = true;

        OnSceneUnloadStarted?.Invoke(activeSceneInstance.Scene.path);

        var loadOperation = Addressables.LoadSceneAsync(sceneAddress);
        activeSceneInstance = await loadOperation.Task;
        OnSceneLoadEnded?.Invoke(sceneAddress);

        await loadingCover.DOFade(0, fadeDuration).AsyncWaitForCompletion();
        loadingCover.raycastTarget = false;

    }
}

public static class SceneDatabase
{
    public const string MAIN_MENU = "Assets/Scenes/MainMenu.unity";
    public const string GAMEPLAY = "Assets/Scenes/Gameplay.unity";
    public const string WIN = "Assets/Scenes/Win.unity";
}