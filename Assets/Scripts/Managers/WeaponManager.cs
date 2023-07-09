using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weaponInventory;
    public Weapon currentWeapon;
    // Start is called before the first frame update
    void Awake()
    {
        UpdateWeaponInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentWeapon.Fire();
        }
        if (currentWeapon.canADS)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                currentWeapon.ChangeADS(true);
            }
            if (Input.GetButtonUp("Fire2"))
            {
                currentWeapon.ChangeADS(false);
            }
        }
        if (Input.GetButtonDown("Reload"))
        {
            currentWeapon.Reload();
        }
    }
    void UpdateWeaponInventory()
    {
        weaponInventory.Clear();
        Weapon[] weaponsUnderManager = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weaponsUnderManager)
        {
            weaponInventory.Add(weapon);
        }
    }
}
