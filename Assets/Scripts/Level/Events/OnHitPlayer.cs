public class OnHitPlayer : IEvent
{
    public int damage = 0;

    public void Set(params object[] data)
    {
        damage = (int)data[0];
    }
    public void Reset()
    {
        damage = 0;
    }
}
