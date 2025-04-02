using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform weaponPoint;

    [SerializeField] private Transform weaponHolder;

    private void Start()
    {
        player = GetComponent<Player>();

        player.actions.Player.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, weaponPoint.position, Quaternion.LookRotation(weaponPoint.forward));

        newBullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10f);

        GetComponentInChildren<Animator>().SetTrigger("Shoot");
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        
        Vector3 direction = (aim.position - weaponPoint.position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
            direction.y = 0;

        // weaponHolder.LookAt(aim);
        // weaponPoint.LookAt(aim);

        return direction;
    }

    public Transform WeaponPoint() => weaponPoint;
}
