using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameStateManager : Singleton<GameStateManager>
{
    private GameState currentState = GameState.Playing;
    private int requireWinGatesToWin = 0;
    private int currentWinGatesActivated = 0;


    public void RegisterWinGate()
    {
        requireWinGatesToWin++;
    }

    public void UnregisterWinGate()
    {
        requireWinGatesToWin--;
    }

    public void ActivateWinGate()
    {
        currentWinGatesActivated++;
        if (currentWinGatesActivated >= requireWinGatesToWin)
        {
            SetGameState(GameState.LevelCompleted);
        }
    }

    public void DeactivateWinGate()
    {
        currentWinGatesActivated--;
    }

    private void SetGameState(GameState newState)
    {
        currentState = newState;

        if (currentState == GameState.LevelCompleted)
        {
            LogService.Log("Level Completed!");
            // Handle level completion logic here
        }
    }
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    LevelCompleted
}