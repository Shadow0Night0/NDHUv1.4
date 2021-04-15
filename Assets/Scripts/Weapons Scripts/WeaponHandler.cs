using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim
{
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType
{
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType
{
    NONE,
    BULLET,
    SPEAR,
    ARROW
}

public class WeaponHandler : MonoBehaviour
{

    private Animator anim;

    public WeaponAim weapon_Aim;

    [SerializeField]
    private GameObject muzzleFlash;

    private AudioSource shootSound, reload_Sound;

    public WeaponFireType fireType;

    public WeaponBulletType bulletType;

    public GameObject attack_Point;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShootAnimation()
    {
        anim.SetTrigger(AnimationTags.SHOOT_TRIGGER);
    }

    public void Aim(bool canAim)
    {
        anim.SetBool(AnimationTags.AIM_TRIGGER, canAim);
    }

    void Turn_On_MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
    }
    void Turn_Off_MuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }

    void Play_ShootSound()
    {
        shootSound.Play();
    }

    void Play_ReloadSound()
    {
        reload_Sound.Play();
    }

    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if(attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}




















































