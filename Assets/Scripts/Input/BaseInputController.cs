
public abstract class BaseInputController
{
    protected InputClass inputClass;

    public BaseInputController()
    {
        inputClass = new InputClass();
        inputClass.Player.Enable();
    }

    public virtual void Dispose()
    {
        inputClass.Player.Disable();
        inputClass.Dispose();
    }
}
