using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private InputSystem_Actions actions;

    [Header("Aim Bilgisi")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;
    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = new Vector3(GetMousePosition().x, transform.position.y +1, GetMousePosition().z);
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    private void AssignInputEvents()
    {
        actions = player.actions;

        actions.Player.Look.performed += context => aimInput = context.ReadValue<Vector2>();
        actions.Player.Look.canceled += context => aimInput = Vector2.zero;
    }
}
