using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtons : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    void OnEnable()
    {
        homeButton?.onClick.AddListener(GoToMainMenu);

        if (restartButton != null)
        {
            InputHandler.Restart += RestartGame;
            restartButton?.onClick.AddListener(RestartGame);
        }
    }

    void OnDisable()
    {
        homeButton?.onClick.RemoveListener(GoToMainMenu);

        if (restartButton != null)
        {
            InputHandler.Restart -= RestartGame;
            restartButton?.onClick.RemoveListener(RestartGame);
        }
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
