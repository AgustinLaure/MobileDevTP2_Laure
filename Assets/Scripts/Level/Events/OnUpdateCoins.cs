public class OnUpdateCoins : IEvent
{
    public int newAmount = 0;

    public void Set(params object[] data)
    {
        newAmount = (int)data[0];
    }
    public void Reset()
    {
        newAmount = 0;
    }
}
