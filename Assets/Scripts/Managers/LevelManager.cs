using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<AssetReference> levelReferences;
    private int currentLevelIndex = -1;

    void Start()
    {
        // Get level data saved, if not load the first level
        LoadLevelWithIndex(0);
    }


    public void LoadLevelWithIndex(int index)
    {
        if (index < 0 || index >= levelReferences.Count)
        {
            LogService.LogError($"Invalid level index: {index}. Please ensure the index is within the range of available levels.");
            return;
        }

        if (currentLevelIndex == index)
        {
            LogService.LogWarning($"Level {index} is already loaded.");
            return;
        }

        if (currentLevelIndex != -1)
            UnloadCurrentLevel();

        var levelReference = levelReferences[index];
        levelReference.LoadAssetAsync<GameObject>().Completed += OnLevelLoaded;
        currentLevelIndex = index;
    }

    private void UnloadCurrentLevel()
    {
        if (currentLevelIndex < 0 || currentLevelIndex >= levelReferences.Count)
        {
            LogService.LogError($"Invalid current level index: {currentLevelIndex}. Cannot unload level.");
            return;
        }

        var currentLevelReference = levelReferences[currentLevelIndex];
        currentLevelReference.ReleaseAsset();
        LogService.Log($"Unloaded level {currentLevelIndex}.");
        currentLevelIndex = -1;
    }

    private void OnLevelLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
            LogService.Log($"Successfully loaded level {currentLevelIndex}.");
        }
        else
        {
            LogService.LogError($"Failed to load level {currentLevelIndex}. Status: {handle.Status}");
        }
    }
}
