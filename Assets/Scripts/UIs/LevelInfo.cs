using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI remainingTurnsLabel;
    [Header("Turn Icons")]
    [SerializeField] private RectTransform turnIconsContainer;
    [SerializeField] private AssetReference turnIconReference;

    [SerializeField] private List<RectTransform> characterTurnIcons = new();

    [Header("Turn Icon Settings")]
    [SerializeField] private List<TurnIconData> turnIconDataList = new();
    private readonly Dictionary<Vector2, Sprite> turnIconDataDictionary = new();
    private float originTurnIconPositionX = 0;


    private readonly Queue<Image> unusedTurnIcons = new();
    private readonly List<Image> activeTurnIcons = new();
    private readonly List<AsyncOperationHandle> turnIconHandles = new();
    private readonly Vector2 blankIconDirection = Vector2.zero;

    private readonly int[] playerMovedDirectionIndexes = new int[2]; // 0: Black, 1: White
    private int currentTurnIconIndex = 0;

    void Awake()
    {
        LoadTurnIconDictionary();
        if (characterTurnIcons.Count > 0)
            originTurnIconPositionX = characterTurnIcons[0].anchoredPosition.x;
    }

    void OnEnable()
    {
        PublicEvents.OnLevelLoaded += HandleLevelLoaded;
        CharacterController.OnTurnLimitChanged += HandleTurnLimitChanged;
        CharacterController.OnRemainingTurnChanged += HandleRemainingTurnChanged;
        CharacterController.OnInputReceived += HandleInputReceived;
        Character.OnCharacterMoved += HandleCharacterMoved;
    }

    void OnDisable()
    {
        PublicEvents.OnLevelLoaded -= HandleLevelLoaded;
        CharacterController.OnTurnLimitChanged -= HandleTurnLimitChanged;
        CharacterController.OnRemainingTurnChanged -= HandleRemainingTurnChanged;
        CharacterController.OnInputReceived -= HandleInputReceived;
        Character.OnCharacterMoved -= HandleCharacterMoved;
    }

    private void HandleLevelLoaded(int levelIndex)
    {
        levelLabel.text = $"Level {levelIndex + 1}";
        playerMovedDirectionIndexes[0] = playerMovedDirectionIndexes[1] = 0;
        foreach (var characterIcon in characterTurnIcons)
        {
            var pos = characterIcon.anchoredPosition;
            pos.x = originTurnIconPositionX;
            characterIcon.anchoredPosition = pos;
        }
        currentTurnIconIndex = 0;
    }

    private void HandleTurnLimitChanged(int newTurnLimit)
    {
        LoadBlankTurnIcon(newTurnLimit).Forget();
    }

    private void HandleRemainingTurnChanged(int remainingTurns)
    {
        remainingTurnsLabel.text = remainingTurns.ToString();
    }

    private async UniTask LoadBlankTurnIcon(int amount)
    {
        for (int i = activeTurnIcons.Count - 1; i >= amount; i--)
        {
            Image iconToDeactivate = activeTurnIcons[i];
            iconToDeactivate.gameObject.SetActive(false);
            unusedTurnIcons.Enqueue(iconToDeactivate);
            activeTurnIcons.RemoveAt(i);
        }

        while (activeTurnIcons.Count < amount)
        {
            if (unusedTurnIcons.Count > 0)
            {
                Image iconToActivate = unusedTurnIcons.Dequeue();
                iconToActivate.gameObject.SetActive(true);
                activeTurnIcons.Add(iconToActivate);
            }
            else
            {
                AsyncOperationHandle<GameObject> handle = turnIconReference.InstantiateAsync(turnIconsContainer);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject newIcon = handle.Result;
                    if (newIcon.TryGetComponent<Image>(out var iconImage))
                    {
                        activeTurnIcons.Add(iconImage);
                        turnIconHandles.Add(handle);
                    }
                    else
                    {
                        Debug.LogError("The instantiated turn icon does not have an Image component.");
                        Addressables.Release(handle);
                    }
                }
                else
                {
                    Debug.LogError("Failed to load turn icon from addressables.");
                }
            }
        }

        foreach (var icon in activeTurnIcons)
        {
            if (turnIconDataDictionary.TryGetValue(blankIconDirection, out var blankSprite))
                icon.sprite = blankSprite;
            else
                Debug.LogWarning("No sprite found for blank direction.");
        }
    }

    private void LoadTurnIconDictionary()
    {
        turnIconDataDictionary.Clear();
        foreach (var turnIconData in turnIconDataList)
        {
            if (!turnIconDataDictionary.ContainsKey(turnIconData.direction))
            {
                turnIconDataDictionary[turnIconData.direction] = turnIconData.directionSprite;
            }
        }
    }

    private void HandleInputReceived(Vector2 input)
    {
        if (currentTurnIconIndex >= activeTurnIcons.Count)
        {
            Debug.LogWarning("Not enough turn icons to display the input.");
            return;
        }

        if (turnIconDataDictionary.TryGetValue(input, out var directionSprite))
            activeTurnIcons[currentTurnIconIndex].sprite = directionSprite;
        else
            Debug.LogWarning($"No sprite found for input direction: {input}");

        currentTurnIconIndex++;
    }

    private void HandleCharacterMoved(CharacterColor characterColor)
    {
        playerMovedDirectionIndexes[(int)characterColor]++;
        var pos = characterTurnIcons[(int)characterColor].anchoredPosition;
        var index = Mathf.Clamp(playerMovedDirectionIndexes[(int)characterColor] - 1, 0, activeTurnIcons.Count - 1);
        pos.x = activeTurnIcons[index].rectTransform.anchoredPosition.x;
        characterTurnIcons[(int)characterColor].anchoredPosition = pos;
    }

    void OnDestroy()
    {
        foreach (var handle in turnIconHandles)
        {
            Addressables.Release(handle);
        }
    }
}

[Serializable]
public struct TurnIconData
{
    public Vector2 direction; // 0, 0 = blank
    public Sprite directionSprite;
}