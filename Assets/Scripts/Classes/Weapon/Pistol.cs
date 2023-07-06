using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] Vector3 hipFirePosition;
    [SerializeField] Vector3 adsPosition;
    private void Awake()
    {
        adsTransitionTime = 0.25f;
        hipFireWeaponPosition = hipFirePosition;
        adsWeaponPosition= adsPosition;
        maxAmmo = 9999;
        reloadTime = 2f;
        clipSize = 10;
    }
}
