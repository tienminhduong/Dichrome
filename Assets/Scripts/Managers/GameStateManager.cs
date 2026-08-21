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
            SetGameState(GameState.LevelWin);
        }
    }

    public void DeactivateWinGate()
    {
        currentWinGatesActivated--;
    }

    private void SetGameState(GameState newState)
    {
        currentState = newState;

        if (currentState == GameState.LevelWin)
        {
            LogService.Log("Level Completed!");
            PublicEvents.RaiseLevelEnded(true);
        }
    }
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    LevelWin
}