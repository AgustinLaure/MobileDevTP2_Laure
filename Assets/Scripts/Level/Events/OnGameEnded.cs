using UnityEngine;

public class OnGameEnded : IEvent
{
    public bool hasWon = false;
    public float timeLeft = 0f;
    public int moneyAmount = 0;

    public void Set(params object[] data)
    {
        hasWon = (bool)data[0];
        timeLeft = (float)data[1];
        moneyAmount = (int)data[2];
    }
    public void Reset()
    {
        hasWon = false;
        timeLeft = 0f;
        moneyAmount = 0;
    }
}
