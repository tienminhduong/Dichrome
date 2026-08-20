using System;
using UnityEngine;

[Serializable]
public class TurnCountdownTimer
{
    [SerializeField] private int remainingTurns;
    [SerializeField] private int totalTurns;
    [SerializeField] private bool resetOnFinish;
    public event Action OnTurnCountdownFinished;

    public int RemainingTurns => remainingTurns;
    public int TotalTurns => totalTurns;

    public TurnCountdownTimer(int totalTurns = 0, bool resetOnFinish = false)
    {
        this.totalTurns = totalTurns;
        this.resetOnFinish = resetOnFinish;
        remainingTurns = totalTurns;
    }

    public void DecrementTurn()
    {
        if (remainingTurns <= 0)
            return;

        remainingTurns--;
        if (remainingTurns == 0)
        {
            OnTurnCountdownFinished?.Invoke();
            if (resetOnFinish)
            {
                ResetTimer();
            }
        }
    }

    public void ResetTimer()
    {
        remainingTurns = totalTurns;
    }

    public void SetTotalTurns(int newTotalTurns)
    {
        totalTurns = newTotalTurns;
        ResetTimer();
    }

    public void SetResetOnFinish(bool shouldReset)
    {
        resetOnFinish = shouldReset;
    }
}