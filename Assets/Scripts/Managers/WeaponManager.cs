using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weaponInventory;
    public Weapon currentWeapon;
    int desiredWeaponIndex = 0;
    bool scrollAxisInUse;
    // Start is called before the first frame update
    void Awake()
    {
        UpdateWeaponInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon != null)
        {
            if (currentWeapon.IsAutomatic)
            {
                if (Input.GetButton("Fire1"))
                {
                    currentWeapon.Fire();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    currentWeapon.Fire();
                }
            }
            if (currentWeapon.canADS)
            {
                currentWeapon.ChangeADS(Input.GetButton("Fire2"));
            }
            if (Input.GetButtonDown("Reload"))
            {
                currentWeapon.Reload();
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            {
                if (!scrollAxisInUse)
                {
                    if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
                    {
                        desiredWeaponIndex++;
                    }
                    else
                    {
                        desiredWeaponIndex--;
                    }
                    if (desiredWeaponIndex > weaponInventory.Count - 1)
                    {
                        desiredWeaponIndex = 0;
                    }
                    if (desiredWeaponIndex < 0)
                    {
                        desiredWeaponIndex = weaponInventory.Count - 1;
                    }


                    scrollAxisInUse = true;
                }
            }
            else
            {
                scrollAxisInUse = false;
            }
        }
        if (desiredWeaponIndex != weaponInventory.IndexOf(currentWeapon) && weaponInventory.ElementAtOrDefault(desiredWeaponIndex) != null && !currentWeapon.isReloading && desiredWeaponIndex < weaponInventory.Count && desiredWeaponIndex >= 0)
        {
            currentWeapon.gameObject.SetActive(false);
            currentWeapon.gameObject.transform.localPosition = currentWeapon.hipFireWeaponPosition;
            currentWeapon = weaponInventory[desiredWeaponIndex];
            currentWeapon.gameObject.SetActive(true);
        }
        
    }
    public void UpdateWeaponInventory()
    {
        weaponInventory.Clear();
        Weapon[] weaponsUnderManager = GetComponentsInChildren<Weapon>(true);
        foreach (Weapon weapon in weaponsUnderManager)
        {
            weaponInventory.Add(weapon);
        }
        if(desiredWeaponIndex > weaponInventory.Count - 1)
        {
            desiredWeaponIndex = weaponInventory.Count - 1;
        }
        if(desiredWeaponIndex< 0)
        {
            desiredWeaponIndex = 0;
        }
        if (weaponInventory.ElementAtOrDefault(desiredWeaponIndex) != null)
        {
            currentWeapon = weaponInventory[desiredWeaponIndex];
            foreach (Weapon weapon in weaponInventory)
            {
                if (weapon != currentWeapon)
                {
                    weapon.gameObject.SetActive(false);
                }
                else
                {
                    weapon.gameObject.SetActive(true);
                }
            }
        }
    }
    
}
