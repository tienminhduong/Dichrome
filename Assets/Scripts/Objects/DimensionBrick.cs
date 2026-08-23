using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[DefaultExecutionOrder(0)]
public class DimensionBrick : BaseSpecialNode
{
    private SpriteRenderer spriteRenderer;
    private Vector3Int gridPosition;

    public static event Action<Vector3Int> OnChangeBrickStateTriggered;
    override public bool IsWalkable => currentState == BrickState.Walkable;


    [SerializeField] private BrickState currentState = BrickState.Walkable;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = DimensionBrickController.Instance.GetSpriteForState(currentState);

        gridPosition = gameGrid.WorldToGridPosition(transform.position);
    }

    void OnEnable()
    {
        OnChangeBrickStateTriggered += HandleChangeBrickState;
    }

    void OnDisable()
    {
        OnChangeBrickStateTriggered -= HandleChangeBrickState;
    }

    public void SwitchState()
    {
        currentState = currentState == BrickState.Walkable ? BrickState.Unwalkable : BrickState.Walkable;
        spriteRenderer.sprite = DimensionBrickController.Instance.GetSpriteForState(currentState);
        spriteRenderer.material = DimensionBrickController.Instance.GetDifferentMaterial(spriteRenderer.sharedMaterial);

        gameGrid.SwitchNodeWalkableState(gridPosition, currentState == BrickState.Walkable);
    }

    public override void OnCharacterExit(Character character)
    {
        base.OnCharacterExit(character);
        OnChangeBrickStateTriggered?.Invoke(gridPosition);
        AudioManager.Instance.PlaySFX(SoundDatabase.LOCK);
    }

    private void HandleChangeBrickState(Vector3Int position)
    {
        if (position == gridPosition)
            SwitchState();
    }
}