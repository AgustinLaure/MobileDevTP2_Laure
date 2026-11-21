using UnityEngine;

public class OnPickCoin : IEvent
{
    public int value = 0;

    public void Set(params object[] data)
    {
        value = (int)data[0];
    }
    public void Reset()
    {
        value = 0;
    }
}
