using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponHandler[] weapons;

    

    private int Axe_Index = 0, Bow_Index = 1, Freehand_Index = 2;

    private int current_Weapon_Index = 2;
    // Start is called before the first frame update




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnSelectedWeapon(Axe_Index);

        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnSelectedWeapon(Bow_Index);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        { 
            weapons[current_Weapon_Index].gameObject.SetActive(false);
            current_Weapon_Index = Freehand_Index;
        }
    }//update

    void TurnOnSelectedWeapon(int weaponIndex)
    {
        if (current_Weapon_Index == weaponIndex)
        {
            Debug.Log("Yo current_Weapon_Index is = weaponIndex EleGiggle");
            return;

        }

        // turn off the current weapon
        weapons[current_Weapon_Index].gameObject.SetActive(false);

        // turn on the selected weapon
        weapons[weaponIndex].gameObject.SetActive(true);

        // store the current selected weapon index
        current_Weapon_Index = weaponIndex;

    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[current_Weapon_Index];

    }

} //class






















