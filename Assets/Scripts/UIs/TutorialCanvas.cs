using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvas : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    [Header("Settings")]
    [SerializeField] private float secondsPerCharacter = 0.05f;

    private readonly Queue<string> dialogueQueue = new();
    private bool isTyping = false;
    private CancellationTokenSource typingCancellationTokenSource;
    private string HTML_TRANSPARENT_TAG = "<color=#00000000>";
    private string HTML_END_TAG = "</color>";

    public void LoadAndShowDialogue(List<string> dialogueLines)
    {
        dialogueQueue.Clear();
        foreach (var line in dialogueLines)
        {
            dialogueQueue.Enqueue(line);
        }

        OpenUI();
        PlayDialogue(dialogueQueue.Dequeue());
    }

    void OnEnable()
    {
        nextButton.onClick.AddListener(HandleDialogueTapped);
    }

    void OnDisable()
    {
        nextButton.onClick.RemoveListener(HandleDialogueTapped);
    }

    private void HandleDialogueTapped()
    {
        if (isTyping)
        {
            EndCurrentDialogueEarly();
            return;
        }

        if (dialogueQueue.Count > 0)
        {
            string nextLine = dialogueQueue.Dequeue();
            PlayDialogue(nextLine);
        }
        else
        {
            CloseUI();
        }
    }

    private void PlayDialogue(string dialogueLine)
    {
        typingCancellationTokenSource = new CancellationTokenSource();
        PlayDialogueAsync(dialogueLine, typingCancellationTokenSource.Token).Forget();

        // dialogueText.text = dialogueLine;
    }

    private void EndCurrentDialogueEarly()
    {
        if (isTyping)
        {
            typingCancellationTokenSource.Cancel();
            isTyping = false;
        }
    }

    private async UniTask PlayDialogueAsync(string dialogueLine, CancellationToken cancellationToken)
    {
        isTyping = true;

        dialogueText.text = string.Empty;
        var originalText = dialogueLine;
        int alphaIndex = 0;

        foreach (char c in originalText)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                alphaIndex++;
                string displayText = originalText.Insert(alphaIndex, HTML_TRANSPARENT_TAG) + HTML_END_TAG;
                dialogueText.text = displayText;

                await UniTask.Delay(TimeSpan.FromSeconds(secondsPerCharacter), cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                isTyping = false;
                dialogueText.text = originalText;
                return;
            }
        }

        isTyping = false;
    }

    private void OpenUI()
    {
        dialoguePanel.SetActive(true);
        nextButton.gameObject.SetActive(true);
        PublicEvents.RaiseUIOpened();
    }

    private void CloseUI()
    {
        dialoguePanel.SetActive(false);
        nextButton.gameObject.SetActive(false);
        PublicEvents.RaiseUIClosed();
    }
}
