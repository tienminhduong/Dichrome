using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button continueButton;

    void OnEnable()
    {
        startButton.onClick.AddListener(StartGame);
        continueButton.onClick.AddListener(ContinueGame);
    }

    void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
        continueButton.onClick.RemoveListener(ContinueGame);
    }

    private void StartGame()
    {
        AudioManager.Instance.PlaySFX(SoundDatabase.BUTTON_CLICK);
        LevelManager.Instance.SetNextLevelToLoad(0);
        SceneController.Instance.LoadAddessableScene(SceneDatabase.GAMEPLAY);
    }

    private void ContinueGame()
    {
        AudioManager.Instance.PlaySFX(SoundDatabase.BUTTON_CLICK);
        int savedLevelIndex = LevelManager.Instance.GetSavedLevelIndex();
        LevelManager.Instance.SetNextLevelToLoad(savedLevelIndex);
        SceneController.Instance.LoadAddessableScene(SceneDatabase.GAMEPLAY);
    }
}
