using UnityEngine;

public class OnSwipe : IEvent
{
    public Vector2 direction = Vector2.zero;

    public void Set(params object[] data)
    {
        direction = (Vector2)data[0];
    }
    public void Reset()
    {
        direction = Vector2.zero;
    }
}
