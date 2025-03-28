using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController weaponVisualController;

    private void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
    }
    
    public void ReloadIsOver()
    {
        weaponVisualController.ReturnRigWeightToOne();
    }

    public void ReturnRig()
    {
        weaponVisualController.ReturnRigWeightToOne();
        weaponVisualController.ReturnWeightToLeftHandIK();
    }

    public void WeaponGrabIsOver()
    {
        weaponVisualController.SetBusyGrabbingWeaponTo(false);
    }
}
