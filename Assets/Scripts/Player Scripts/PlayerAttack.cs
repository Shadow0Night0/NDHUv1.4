using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    public float nextTimeToFire;
    public float damage = 20f;

    private void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
    }

    void WeaponShoot()
    {
        // if we have an assault rifle
        if (weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            // if we press and hold left mouse click AND
            // if Time is greater than the nextTimeToFire
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                //BulletFired();
            }
        }
        // if we have a regular weapon that shoots once
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                // handle Axe
                if(weapon_Manager.GetCurrentSelectedWeapon().CompareTag(Tags.AXE_TAG))
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                //handle shoot
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                    //BulletFired();

                }
                else
                {
                    //we have arrow or spear

                } // else
            }
        }

    }

} // class

























