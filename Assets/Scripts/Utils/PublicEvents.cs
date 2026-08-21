using System;

public static class PublicEvents
{
    public static event Action OnUIOpened;
    public static event Action OnUIClosed;

    public static void RaiseUIOpened()
    {
        OnUIOpened?.Invoke();
    }

    public static void RaiseUIClosed()
    {
        OnUIClosed?.Invoke();
    }

    public static event Action<bool> OnLevelEnded; // true for win, false for lose
    public static void RaiseLevelEnded(bool isWin)
    {
        OnLevelEnded?.Invoke(isWin);
    }

    public static event Action<int> OnLevelLoaded;
    public static void RaiseLevelLoaded(int levelIndex)
    {
        OnLevelLoaded?.Invoke(levelIndex);
    }
}