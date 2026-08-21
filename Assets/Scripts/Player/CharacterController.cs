using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private List<Character> characters = new();
    [SerializeField] private int turnLimit = 10;

    private bool isLockMovement = false;
    private Character lockCalledCharacter = null;
    private int currentTurn = 0;

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

    private void Start()
    {
        foreach (var character in characters)
        {
            character.Initialize(this);
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

        foreach (var character in characters)
        {
            character.QueueMovement(input);
        }

        CheckEndTurn();
    }

    private void CheckEndTurn()
    {
        currentTurn++;
        if (currentTurn >= turnLimit)
        {
            PublicEvents.RaiseLevelEnded(false);
        }
    }
}
