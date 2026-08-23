using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtons : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    void OnEnable()
    {
        restartButton?.onClick.AddListener(RestartGame);
        homeButton?.onClick.AddListener(GoToMainMenu);
    }

    void OnDisable()
    {
        restartButton?.onClick.RemoveListener(RestartGame);
        homeButton?.onClick.RemoveListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        AudioManager.Instance.PlaySFX(SoundDatabase.BUTTON_CLICK);
        SceneController.Instance.LoadAddessableScene(SceneDatabase.MAIN_MENU);
    }

    private void RestartGame()
    {
        AudioManager.Instance.PlaySFX(SoundDatabase.BUTTON_CLICK);
        LevelManager.Instance.ReloadCurrentLevel();
    }
}
