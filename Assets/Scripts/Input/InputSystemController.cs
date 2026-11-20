using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemController : BaseInputController
{
    private EventBus eventBus;

    public InputSystemController() : base()
    {
        inputClass.Player.Swipe.performed += HandleSwipe;

        eventBus = ServiceLocator.Instance.GetService<EventBus>();
    }

    private void HandleSwipe(InputAction.CallbackContext context)
    {
        eventBus.Raise<OnSwipe>(context.ReadValue<Vector2>());
    }

    public override void Dispose()
    {
        inputClass.Player.Swipe.performed -= HandleSwipe;
        base.Dispose();
    }
}
