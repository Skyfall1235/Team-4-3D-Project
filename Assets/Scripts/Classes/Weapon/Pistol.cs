using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] Vector3 hipFirePosition;
    [SerializeField] Vector3 adsPosition;
    [SerializeField] int weaponPenetrationPower = 0;
    [SerializeField] float maxFireDistance = 1200f;
    [SerializeField] LayerMask bulletLayerMask;
    [SerializeField] int weaponDamage = 20;
    [SerializeField] Vector3 startingMaxHipFireRecoilAmounts;
    [SerializeField] Vector3 startingMaxADSRecoilAmounts;
    [SerializeField] Vector2 startingMaxHipFireWeaponInaccuracy;
    [SerializeField] Vector2 startingMaxADSWeaponInaccuracy;
    [SerializeField] float startingRecoilSnapiness;
    [SerializeField] float startingRecoilReturnSpeed;
    Camera playerCam;
    float remainingPenetrations;
    public override void Fire()
    {
        if(!isReloading)
        {
            if(SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundOnObject(gameObject, "Pistol Shot", false);
            }
            Vector3 hipFireShotDeviation = (playerCam.transform.up * Random.Range(-maxHipFireWeaponInaccuracy.x, maxHipFireWeaponInaccuracy.x)) + (playerCam.transform.right * Random.Range(-startingMaxHipFireWeaponInaccuracy.y, startingMaxHipFireWeaponInaccuracy.y));
            Vector3 adsShotdeviation = (playerCam.transform.up * Random.Range(-maxADSWeaponInaccuracy.x, maxADSWeaponInaccuracy.x)) + (playerCam.transform.right * Random.Range(-maxADSWeaponInaccuracy.y, maxADSWeaponInaccuracy.y));
            RaycastHit[] bulletHits = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward + (isADS ? adsShotdeviation : hipFireShotDeviation), maxFireDistance, bulletLayerMask);
            SortedDictionary<float, RaycastHit> raycastHitDistances = new SortedDictionary<float, RaycastHit>();
            foreach (RaycastHit hit in bulletHits)
            {
                raycastHitDistances.Add(Vector3.Distance(playerCam.transform.position, hit.point), hit);
            }
            remainingPenetrations = weaponPenetrationPower;
            for (int i = 0; i < raycastHitDistances.Count && remainingPenetrations > 0; i++)
            {
                if (raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<IDamagable>() != null)
                {
                    raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<IDamagable>().Damage(weaponDamage);
                }
                if(raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * 2, ForceMode.Impulse);
                }
                remainingPenetrations--;
            }
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
        maxHipFireRecoilAmounts = startingMaxHipFireRecoilAmounts;
        maxADSRecoilAmounts = startingMaxADSRecoilAmounts;
        recoilSnappiness = startingRecoilSnapiness;
        recoilReturnSpeed = startingRecoilReturnSpeed;
        maxHipFireWeaponInaccuracy = startingMaxHipFireWeaponInaccuracy;
        maxADSWeaponInaccuracy =  startingMaxADSWeaponInaccuracy;
    }
}
