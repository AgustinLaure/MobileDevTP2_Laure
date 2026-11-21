using UnityEngine;

public class OnUpdateHp : IEvent
{
    public int maxHp = 0;
    public int hp = 0;

    public void Set(params object[] data)
    {
        maxHp = (int)data[0];
        hp = (int)data[1];
    }
    public void Reset()
    {
        maxHp = 0;
        hp = 0;
    }
}
