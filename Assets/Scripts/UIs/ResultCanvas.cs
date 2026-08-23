using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvas : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private float delayBeforeShowingResult = 0.5f;

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
        resultPanel.SetActive(false);

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
        AudioManager.Instance.PlaySFX(SoundDatabase.BUTTON_CLICK);
        SceneController.Instance.LoadAddessableScene(SceneDatabase.MAIN_MENU);

        TurnOffPanel();
    }

    private void OnRestartButtonClicked()
    {
        AudioManager.Instance.PlaySFX(SoundDatabase.BUTTON_CLICK);
        if (LevelManager.HasInstance)
            LevelManager.Instance.ReloadCurrentLevel();

        TurnOffPanel();
    }

    private void OnNextLevelButtonClicked()
    {
        AudioManager.Instance.PlaySFX(SoundDatabase.BUTTON_CLICK);
        if (LevelManager.HasInstance)
            LevelManager.Instance.LoadNextLevel();

        TurnOffPanel();
    }

    private void ShowResult(bool isWin)
    {
        PublicEvents.RaiseUIOpened();

        UniTask.Delay(TimeSpan.FromSeconds(delayBeforeShowingResult)).ContinueWith(() =>
        {
            resultPanel.SetActive(true);
            if (isWin)
                AudioManager.Instance.PlaySFX(SoundDatabase.WIN);
            else
                AudioManager.Instance.PlaySFX(SoundDatabase.LOSE);
            resultText.text = isWin ? winMessage : loseMessage;
            nextLevelButton.gameObject.SetActive(isWin);
        });
    }

    private void TurnOffPanel()
    {
        resultPanel.SetActive(false);
        PublicEvents.RaiseUIClosed();
    }
}
