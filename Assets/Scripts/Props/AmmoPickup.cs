using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer)), RequireComponent(typeof(Collider))]
public class AmmoPickup : MonoBehaviour, IResetable
{
    [SerializeField, Range(0, 1)] float refillPercentage;

    Renderer rend;
    Collider coll;
    void Start()
    {
        if(GetComponent<Renderer>() != null)
        {
            rend= GetComponent<Renderer>();
        }
        if(GetComponent<Collider>() != null)
        {
            coll= GetComponent<Collider>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.GetComponentInChildren<WeaponManager>() != null)
        {
            foreach(Weapon weapon in other.transform.root.GetComponentInChildren<WeaponManager>().weaponInventory)
            {
                weapon.currentAmmo = Mathf.Clamp((int)(weapon.currentAmmo + weapon.currentMaxAmmo * refillPercentage), 0, weapon.currentMaxAmmo);
            }
            if(coll != null)
            {
                coll.enabled= false;
            }
            if(rend != null)
            {
                rend.enabled= false;
            }
        }
    }
    public void ResetObject()
    {
        if (coll != null)
        {
            coll.enabled = true;
        }
        if (rend != null)
        {
            rend.enabled = true;
        }
    }
}
