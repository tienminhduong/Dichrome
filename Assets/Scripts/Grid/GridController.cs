using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private List<GameGrid> gameGrids = new();
    private int activeGridIndex = 0;

    void OnEnable()
    {
        InputHandler.Swap += SwitchToNextGrid;
    }

    void OnDisable()
    {
        InputHandler.Swap -= SwitchToNextGrid;
    }

    private int GetNextGridIndex()
    {
        return (activeGridIndex + 1) % gameGrids.Count;
    }

    public void SwitchToNextGrid()
    {
        int nextGridIndex = GetNextGridIndex();
        (gameGrids[nextGridIndex].transform.position, gameGrids[activeGridIndex].transform.position)
            = (gameGrids[activeGridIndex].transform.position, gameGrids[nextGridIndex].transform.position);
        activeGridIndex = nextGridIndex;
    }
}
