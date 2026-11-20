using UnityEngine;

public class OnGameEnded : IEvent
{
    public bool hasWon = false;

    public void Set(params object[] data)
    {
        hasWon = (bool)data[0];
    }
    public void Reset()
    {
        hasWon = false;
    }
}
