using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] Vector3 hipFirePosition;
    [SerializeField] Vector3 adsPosition;
    [SerializeField] int weaponPenetrationPower = 0;
    [SerializeField] float maxFireDistance = 1200f;
    [SerializeField] LayerMask bulletLayerMask;
    [SerializeField] int weaponDamage = 20;
    Camera playerCam;
    float remainingPenetrations;
    public override void Fire()
    {
        RaycastHit[] bulletHits = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward, maxFireDistance, bulletLayerMask);
        SortedDictionary<float, RaycastHit> raycastHitDistances = new SortedDictionary<float, RaycastHit>();
        foreach (RaycastHit hit in bulletHits)
        {
            raycastHitDistances.Add(Vector3.Distance(playerCam.transform.position, hit.point), hit);
        }
        remainingPenetrations = weaponPenetrationPower;
        for(int i = 0; i < raycastHitDistances.Count && remainingPenetrations > 0; i++)
        {
            if(raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<IDamagable>() != null)
            {
                raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<IDamagable>().Damage(weaponDamage);
            }
            remainingPenetrations--;
        }
        base.Fire();
    }
    private void Awake()
    {
        playerCam = Camera.main;
        adsTransitionTime = 0.25f;
        hipFireWeaponPosition = hipFirePosition;
        adsWeaponPosition= adsPosition;
        maxAmmo = 9999;
        reloadTime = 2f;
        clipSize = 10;
    }
}
