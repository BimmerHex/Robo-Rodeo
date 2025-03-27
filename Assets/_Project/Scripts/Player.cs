using UnityEngine;

public class Player : MonoBehaviour
{
    public InputSystem_Actions playerActions;

    private void Awake()
    {
        playerActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
