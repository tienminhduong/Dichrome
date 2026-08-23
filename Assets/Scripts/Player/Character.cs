using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int MoveLeftHash = Animator.StringToHash("MoveLeft");
    private static readonly int MoveRightHash = Animator.StringToHash("MoveRight");
    private static readonly int MoveUpDownHash = Animator.StringToHash("MoveUpDown");
    private readonly Queue<Vector2> movementQueue = new();
    [SerializeField] private TurnCountdownTimer movementTimer = new(1, true);
    [SerializeField] private CharacterColor characterColor = CharacterColor.Black;
    public CharacterColor CharacterColor => characterColor;
    public static event Action<CharacterColor> OnCharacterMoved;

    private Tween moveTween;
    private CharacterController controller;

    private GameGrid gameGrid;
    private Animator animator;
    private Vector3 normalScale;

    void Awake()
    {
        gameGrid = transform.parent.GetComponent<GameGrid>();
        animator = GetComponent<Animator>();
        if (gameGrid == null)
            LogService.LogError($"{gameObject.name} is not a child of a GameGrid. Please ensure that the Character is placed under a GameGrid in the hierarchy.");
    }

    void OnEnable()
    {
        movementTimer.OnTurnCountdownFinished += ProcessMovement;
    }

    void OnDisable()
    {
        movementTimer.OnTurnCountdownFinished -= ProcessMovement;
    }

    public void Initialize(CharacterController characterController)
    {
        controller = characterController;
    }

    public void ChangeGrid(GameGrid newGrid)
    {
        gameGrid = newGrid;
        transform.SetParent(newGrid.transform);
    }

    public void QueueMovement(Vector2 input)
    {
        movementQueue.Enqueue(input);
        movementTimer.DecrementTurn();
    }

    private void ProcessMovement()
    {
        if (movementQueue.Count > 0)
        {
            Vector2 input = movementQueue.Dequeue();
            OnCharacterMoved?.Invoke(characterColor);
            Vector3Int currentGridPosition = gameGrid.WorldToGridPosition(transform.position);
            Vector3Int nextGridPosition = currentGridPosition + new Vector3Int((int)input.x, (int)input.y, 0);
            Vector3 targetWorldPosition = gameGrid.GridToWorldPosition(nextGridPosition);

            bool isWalkable = gameGrid.IsWalkable(nextGridPosition);
            if (isWalkable)
            {
                controller.RaiseLockMovement(this);
                gameGrid.HandleMoveOutPos(currentGridPosition, this);
                SmoothMove(targetWorldPosition, onComplete: () =>
                {
                    controller.ReleaseLockMovement(this);
                    gameGrid.HandleMoveToPos(nextGridPosition, this);
                });
            }
        }
    }

    private void SmoothMove(Vector3 targetPosition, TweenCallback onComplete = null)
    {
        moveTween?.Kill(true);
        var direction = targetPosition - transform.position;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                animator.Play(MoveRightHash);
            else
                animator.Play(MoveLeftHash);
        }
        else
        {
            animator.Play(IdleHash);
        }
        moveTween = transform.DOMove(targetPosition, CommonSetting.Instance.MoveAnimTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.position = targetPosition;
                animator.Play(IdleHash);
                onComplete?.Invoke();
            });
    }

    public Tween PlayScaleDownTween()
    {
        moveTween?.Kill(true);
        normalScale = transform.localScale;
        return transform.DOScale(Vector3.zero, CommonSetting.Instance.MoveAnimTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                transform.localScale = Vector3.zero;
            });
    }

    public Tween PlayScaleUpTween()
    {
        moveTween?.Kill(true);
        transform.localScale = Vector3.zero;
        return transform.DOScale(normalScale, CommonSetting.Instance.MoveAnimTime)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                transform.localScale = normalScale;
            });
    }

    public void SetMovementTimer(int totalTurns)
    {
        movementTimer.SetTotalTurns(totalTurns);
    }
}

public enum CharacterColor
{
    Black,
    White
}