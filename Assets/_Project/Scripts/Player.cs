using UnityEngine;

public class Player : MonoBehaviour
{
    public InputSystem_Actions actions { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponController weaponController { get; private set; }

    private void Awake()
    {
        actions = new InputSystem_Actions();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weaponController = GetComponent<PlayerWeaponController>();
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
