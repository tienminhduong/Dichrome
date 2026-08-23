using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private List<Character> characters = new();
    [SerializeField] private int turnLimit = 10;
    private Level level;

    private bool isLockMovement = false;
    private Character lockCalledCharacter = null;
    private int currentTurn = 0;

    public static event Action<int> OnTurnLimitChanged;
    public static event Action<int> OnRemainingTurnChanged;
    public static event Action<Vector2> OnInputReceived;

    public void RaiseLockMovement(Character character)
    {
        isLockMovement = true;
        lockCalledCharacter = character;
    }

    public void ReleaseLockMovement(Character character)
    {
        if (lockCalledCharacter == character)
        {
            isLockMovement = false;
            lockCalledCharacter = null;
        }
    }

    private void SetTurnLimit(int limit)
    {
        turnLimit = limit;
        OnTurnLimitChanged?.Invoke(turnLimit);
        OnRemainingTurnChanged?.Invoke(turnLimit - currentTurn);
    }

    public void Initialize(Level level)
    {
        this.level = level;
        SetTurnLimit(level.LevelConfigData.turnLimit);
        foreach (var character in characters)
        {
            character.Initialize(this);
            if (character.CharacterColor == CharacterColor.Black)
                character.SetMovementTimer(level.LevelConfigData.blackPlayerDelay);
            else if (character.CharacterColor == CharacterColor.White)
                character.SetMovementTimer(level.LevelConfigData.whitePlayerDelay);
        }
    }

    void OnEnable()
    {
        InputHandler.Move += HandleMovement;
    }

    void OnDisable()
    {
        InputHandler.Move -= HandleMovement;
    }

    private void HandleMovement(Vector2 input)
    {
        if (isLockMovement || InputHandler.IsInputLocked)
            return;
        if (currentTurn >= turnLimit)
            return;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            input.y = 0;
            input.x = Mathf.Sign(input.x);
        }
        else if (Mathf.Abs(input.y) > Mathf.Abs(input.x))
        {
            input.x = 0;
            input.y = Mathf.Sign(input.y);
        }
        else
        {
            input = Vector2.zero;
        }

        if (input == Vector2.zero)
            return;

        foreach (var character in characters)
        {
            character.QueueMovement(input);
        }
        OnInputReceived?.Invoke(input);
        AudioManager.Instance.PlaySFX(SoundDatabase.MOVE);

        CheckEndTurn();
    }

    private void CheckEndTurn()
    {
        currentTurn++;
        OnRemainingTurnChanged?.Invoke(turnLimit - currentTurn);
        if (currentTurn >= turnLimit)
        {
            UniTask.WaitForSeconds(1).ContinueWith(() =>
            {
                if (GameStateManager.Instance.CurrentState != GameState.LevelWin)
                    PublicEvents.RaiseLevelEnded(false);
            });
        }
    }
}
