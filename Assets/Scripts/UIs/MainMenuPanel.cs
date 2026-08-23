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
        LevelManager.Instance.SetNextLevelToLoad(0);
        SceneController.Instance.LoadAddessableScene(SceneDatabase.GAMEPLAY);
    }

    private void ContinueGame()
    {
        LogService.Log("YOU GET AHAED! AHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAH");
    }
}
