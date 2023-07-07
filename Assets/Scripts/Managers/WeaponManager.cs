using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentGun;
    
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentGun.Fire();
        }
        if (currentGun.canADS)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                currentGun.ChangeADS(true);
            }
            if (Input.GetButtonUp("Fire2"))
            {
                currentGun.ChangeADS(false);
            }
        }
        if (Input.GetButtonDown("Reload"))
        {
            currentGun.Reload();
        }
    }
}
