
public class OnUpdateTimer : IEvent
{
    public float value = 0;

    public void Set(params object[] data)
    {
        value = (float)data[0];
    }
    public void Reset()
    {
        value = 0;
    }
}
