using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvas : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;

    [Header("Messages")]
    [SerializeField] private string winMessage = "You Win!";
    [SerializeField] private string loseMessage = "You Lose!";

    void OnEnable()
    {
        homeButton.onClick.AddListener(OnHomeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);

        PublicEvents.OnLevelEnded += ShowResult;
    }

    void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnHomeButtonClicked);
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);

        PublicEvents.OnLevelEnded -= ShowResult;
    }

    private void OnHomeButtonClicked()
    {
        TurnOffPanel();
    }

    private void OnRestartButtonClicked()
    {
        if (LevelManager.HasInstance)
            LevelManager.Instance.ReloadCurrentLevel();

        TurnOffPanel();
    }

    private void OnNextLevelButtonClicked()
    {
        if (LevelManager.HasInstance)
            LevelManager.Instance.LoadNextLevel();

        TurnOffPanel();
    }

    private void ShowResult(bool isWin)
    {
        resultPanel.SetActive(true);
        resultText.text = isWin ? winMessage : loseMessage;
        nextLevelButton.gameObject.SetActive(isWin);

        PublicEvents.RaiseUIOpened();
    }

    private void TurnOffPanel()
    {
        resultPanel.SetActive(false);
        PublicEvents.RaiseUIClosed();
    }
}
