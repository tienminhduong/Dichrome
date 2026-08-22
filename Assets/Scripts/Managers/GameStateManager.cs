using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameStateManager : Singleton<GameStateManager>
{
    private GameState currentState = GameState.Playing;
    [SerializeField] private int requireWinGatesToWin = 0;
    private int currentWinGatesActivated = 0;

    void OnEnable()
    {
        PublicEvents.OnLevelLoaded += HandleLevelLoaded;
    }

    void OnDisable()
    {
        PublicEvents.OnLevelLoaded -= HandleLevelLoaded;
    }

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

    private void HandleLevelLoaded(int levelIndex)
    {
        currentState = GameState.Playing;
        currentWinGatesActivated = 0;
    }
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    LevelWin
}