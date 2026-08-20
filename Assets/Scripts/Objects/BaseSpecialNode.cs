using UnityEngine;

public abstract class BaseSpecialNode : MonoBehaviour
{
    protected GameGrid gameGrid;
    void Awake()
    {
        gameGrid = transform.parent.GetComponent<GameGrid>();
        if (gameGrid == null)
            LogService.LogError($"{gameObject.name} is not a child of a GameGrid. Please ensure that the Special Node is placed under a GameGrid in the hierarchy.");

        EnableNode();
    }

    public void EnableNode()
    {
        gameGrid.AddSpecialGridNode(this);
    }

    public void DisableNode()
    {
        gameGrid.RemoveSpecialGridNode(this);
    }

    public abstract void OnCharacterEnter(Character character);
}