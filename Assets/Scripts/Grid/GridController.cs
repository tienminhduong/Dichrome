using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private List<GameGrid> gameGrids = new();
    [SerializeField] private float rotationDuration = 0.5f;
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
        if (InputHandler.IsInputLocked)
            return;

        InputHandler.SetLockInput(true);

        int nextGridIndex = GetNextGridIndex();
        var activeGrid = gameGrids[activeGridIndex];
        var nextGrid = gameGrids[nextGridIndex];

        // tween rotate half of the grid to 90 degrees, then swap positions, then tween rotate back to 0 degrees
        nextGrid.transform.localRotation = Quaternion.Euler(0, -90, 0);
        activeGrid.transform.DORotate(new Vector3(0, 90, 0), rotationDuration).OnComplete(() =>
        {
            (nextGrid.transform.position, activeGrid.transform.position)
                = (activeGrid.transform.position, nextGrid.transform.position);
            nextGrid.transform.DORotate(Vector3.zero, rotationDuration).OnComplete(() =>
            {
                activeGrid.transform.localRotation = Quaternion.Euler(0, 0, 0);
                nextGrid.transform.localRotation = Quaternion.Euler(0, 0, 0);
                InputHandler.SetLockInput(false);
            });
        });

        activeGridIndex = nextGridIndex;
    }
}
