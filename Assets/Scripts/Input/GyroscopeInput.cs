using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class GyroscopeInput : OnScreenControl
{
    [InputControl(layout = "Vector2")]
    [SerializeField] private string m_ControlPath;

    [SerializeField] private float flickMagnitude;
    [SerializeField] private float flickCooldown;

    private Coroutine flickCoroutine = null;

    private WaitForSeconds flickCooldownTimer;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (Gyroscope.current != null)
        {
            InputSystem.EnableDevice(Gyroscope.current);
        }
    }
    protected override void OnDisable()
    {
        if (Gyroscope.current != null && Gyroscope.current.enabled)
        {
            InputSystem.DisableDevice(Gyroscope.current);
        }

        base.OnDisable();
    }

    private void Awake()
    {
        flickCooldownTimer = new WaitForSeconds(flickCooldown);
    }

    private void Update()
    {
        if (Gyroscope.current == null || !Gyroscope.current.enabled)
        {
            return;
        }

        if (flickCoroutine == null)
        {
            Vector3 angularVelocity = Gyroscope.current.angularVelocity.ReadValue();

            Vector2 inputDirection = Vector2.zero;

            if (angularVelocity.x >= flickMagnitude)
            {
                inputDirection.y = -1f;
               
            }
            else if (angularVelocity.x <= -flickMagnitude)
            {
                inputDirection.y = 1f;
            }
            else if (angularVelocity.y >= flickMagnitude)
            {
                inputDirection.x = 1f;
            }
            else if (angularVelocity.y <= -flickMagnitude)
            {
                inputDirection.x = -1f;
                
            }

            if (inputDirection != Vector2.zero)
            {
                SendValueToControl(inputDirection);
                SendValueToControl(Vector2.zero);
                flickCoroutine = StartCoroutine(FlickCoroutine());
            }
        }
    }

    private IEnumerator FlickCoroutine()
    {
        yield return flickCooldownTimer;
        flickCoroutine = null;
    }
}
