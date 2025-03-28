using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform[] weaponTransform;

    [SerializeField] private Transform weaponPistol;
    [SerializeField] private Transform weaponRevolver;
    [SerializeField] private Transform weaponRifle;
    [SerializeField] private Transform weaponShotgun;
    [SerializeField] private Transform weaponSniper;

    private Transform currentWeapon;

    [Header("Rig")]
    [SerializeField] private float rightIncreaseStep;
    private bool rigShouldBeIncreased;

    [Header("Left Hand IK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform lefHandIK_Target;
    [SerializeField] private float leftHandIK_IncreaseStep;
    private bool shouldIncreaseLeftHandIKWeight;

    private Rig rig;

    private bool busyGrabbingWeapon;

    private void Start()
    {
        SwitchOnWeapon(weaponPistol);

        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && busyGrabbingWeapon == false)
        {
            anim.SetTrigger("Reload");
            PauseRig();
        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncreaseLeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIK_IncreaseStep * Time.deltaTime;

            if (leftHandIK.weight >= 1)
                shouldIncreaseLeftHandIKWeight = false;
        }
    }

    private void UpdateRigWeight()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rightIncreaseStep * Time.deltaTime;

            if (rig.weight >= 1)
                rigShouldBeIncreased = false;
        }
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0f;
        PauseRig();
        anim.SetFloat("WeaponGrabType", ((float)grabType));
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        busyGrabbingWeapon = busy;
        anim.SetBool("BusyGrabbingWeapon", busyGrabbingWeapon);
    }

    public void ReturnRigWeightToOne() => rigShouldBeIncreased = true;

    public void ReturnWeightToLeftHandIK() => shouldIncreaseLeftHandIKWeight = true;

    private void SwitchOnWeapon(Transform weaponTransform)
    {
        SwitchOffWeapons();
        weaponTransform.gameObject.SetActive(true);
        currentWeapon = weaponTransform;

        AttachLeftHand();
    }

    private void SwitchOffWeapons()
    {
        for (int i = 0; i < weaponTransform.Length; i++)
        {
            weaponTransform[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentWeapon.GetComponentInChildren<LeftHandTargetTransform>().transform;

        lefHandIK_Target.localPosition = targetTransform.localPosition;
        lefHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }

    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOnWeapon(weaponPistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOnWeapon(weaponRevolver);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOnWeapon(weaponRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOnWeapon(weaponShotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOnWeapon(weaponSniper);
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}

public enum GrabType
{
    SideGrab,
    BackGrab
};