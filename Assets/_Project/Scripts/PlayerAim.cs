using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private InputSystem_Actions actions;

    [Header("Aim Görsel - Lazer")]
    [SerializeField] private LineRenderer aimLaser;

    [Header("Aim Kontrol")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget;

    [Header("Kamera Kontrol")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] [Range(0.5f, 1f)] private float minCameraDistance = 1f;
    [SerializeField] [Range(1f, 3f)] private float maxCameraDistance = 3f;
    [SerializeField] [Range(3f, 5f)] private float cameraSensetivity = 4f;

    [Space]

    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 mouseInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            isAimingPrecisly = !isAimingPrecisly;

        if (Input.GetKeyDown(KeyCode.L))
            isLockingToTarget = !isLockingToTarget;

        UpdateAimVisuals();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    private void UpdateAimVisuals()
    {
        Transform weaponPoint = player.weaponController.WeaponPoint();
        Vector3 laserDirection = player.weaponController.BulletDirection();

        float laserTipLength = 0.5f;
        float weaponDistance = 4f;

        Vector3 endPoint = weaponPoint.position + laserDirection * weaponDistance;

        if (Physics.Raycast(weaponPoint.position, laserDirection, out RaycastHit hitInfo, weaponDistance))
        {
            endPoint = hitInfo.point;
            laserTipLength = 0f;
        }

        aimLaser.SetPosition(0, weaponPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }

    private void UpdateAimPosition()
    {
        Transform target = Target();

        if (target != null && isLockingToTarget)
        {
            aim.position = target.position;
            return;
        }

        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }

    public Transform Target()
    {
        Transform target = null;

        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        return target;
    }

    public Transform Aim() => aim;

    public bool CanAimPrecisly() => isAimingPrecisly;

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }

    #region Camera Region
    private Vector3 DesiredCameraPosition()
    {
        float actualMaxCameraDistance = player.movement.moveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }
    #endregion

    private void AssignInputEvents()
    {
        actions = player.actions;

        actions.Player.Look.performed += context => mouseInput = context.ReadValue<Vector2>();
        actions.Player.Look.canceled += context => mouseInput = Vector2.zero;
    }
}
