using UnityEngine;

public class Player : MonoBehaviour
{
    public InputSystem_Actions actions { get; private set; }
    public PlayerAim aim { get; private set; } // Read-Only

    private void Awake()
    {
        actions = new InputSystem_Actions();
        aim = GetComponent<PlayerAim>();
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}
