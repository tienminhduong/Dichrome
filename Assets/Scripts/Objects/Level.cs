using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private LevelConfigData levelConfigData = new()
    {
        turnLimit = 10,
        blackPlayerDelay = 1,
        whitePlayerDelay = 1
    };
    [SerializeField] private CharacterController characterController;

    public LevelConfigData LevelConfigData => levelConfigData;

    void Start()
    {
        characterController.Initialize(this);
    }
}

[Serializable]
public struct LevelConfigData
{
    public int turnLimit;
    public int blackPlayerDelay; // after xxx input, only that the player can move
    public int whitePlayerDelay;
}