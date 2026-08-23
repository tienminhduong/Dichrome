using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtons : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    void OnEnable()
    {
        restartButton.onClick.AddListener(RestartGame);
    }

    void OnDisable()
    {
        restartButton.onClick.RemoveListener(RestartGame);
    }

    private void RestartGame()
    {
        LevelManager.Instance.ReloadCurrentLevel();
    }
}
