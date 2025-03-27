using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] weaponTransform;

    [SerializeField] private Transform weaponPistol;
    [SerializeField] private Transform weaponRevolver;
    [SerializeField] private Transform weaponRifle;
    [SerializeField] private Transform weaponShotgun;
    [SerializeField] private Transform weaponSniper;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchOnWeapon(weaponPistol);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchOnWeapon(weaponRevolver);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchOnWeapon(weaponRifle);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchOnWeapon(weaponShotgun);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            SwitchOnWeapon(weaponSniper);
    }

    private void SwitchOnWeapon(Transform weaponTransform)
    {
        SwitchOffWeapons();
        weaponTransform.gameObject.SetActive(true);
    }

    private void SwitchOffWeapons()
    {
        for (int i = 0; i < weaponTransform.Length; i++)
        {
            weaponTransform[i].gameObject.SetActive(false);
        }
    }
}
