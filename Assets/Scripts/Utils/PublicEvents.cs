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
}