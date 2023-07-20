using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssaultRifle : Weapon
{
    int remainingPenetrations;
    bool canFire = true;
    public override void Fire()
    {
        //If we are not reloading and our clip has bullets in it
        if (!isReloading && currentClip > 0 && canFire)
        {
            ParticleSystem[] muzzleFlashes = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in muzzleFlashes)
            {
                ps.Play();
            }
            //play fire sound
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundOnObject(gameObject, "AR Shot", false);
            }
            //calculate deviation of shot
            Vector3 hipFireShotDeviation = (playerCam.transform.up * Random.Range(-maxHipFireWeaponInaccuracy.x, maxHipFireWeaponInaccuracy.x)) + (playerCam.transform.right * Random.Range(-maxHipFireWeaponInaccuracy.y, maxHipFireWeaponInaccuracy.y));
            Vector3 adsShotdeviation = (playerCam.transform.up * Random.Range(-maxADSWeaponInaccuracy.x, maxADSWeaponInaccuracy.x)) + (playerCam.transform.right * Random.Range(-maxADSWeaponInaccuracy.y, maxADSWeaponInaccuracy.y));
            //Do a raycast and add the results to tthe array
            RaycastHit[] bulletHits = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward + (isADS ? adsShotdeviation : hipFireShotDeviation), maxFireDistance, bulletLayerMask, QueryTriggerInteraction.Ignore);
            //create a sorted dictionary to add our raycast hits to so we can do bullet penetration
            SortedDictionary<float, RaycastHit> raycastHitDistances = new SortedDictionary<float, RaycastHit>();
            //add the raycast hit distances and the corrosponding raycast hit to the sorted dictionary so we can go through the elements and decrease our remaining penetrations after each hit
            foreach (RaycastHit hit in bulletHits)
            {
                if (!raycastHitDistances.ContainsKey(Vector3.Distance(playerCam.transform.position, hit.point)))
                {
                    raycastHitDistances.Add(Vector3.Distance(playerCam.transform.position, hit.point), hit);
                }
            }
            remainingPenetrations = weaponPenetrationPower;
            //Each time the bullet comes into contact with something, decrease its penetration amount and add required effects
            List<GameObject> gameObjectsHit = new List<GameObject>();
            for (int i = 0; i < raycastHitDistances.Count && remainingPenetrations > 0; i++)
            {
                if (!gameObjectsHit.Contains(raycastHitDistances.ElementAt(i).Value.collider.transform.root.gameObject))
                {
                    if (raycastHitDistances.ElementAt(i).Value.collider.gameObject.transform.root.GetComponentInChildren<IDamagable>() != null)
                    {
                        raycastHitDistances.ElementAt(i).Value.collider.gameObject.transform.root.GetComponentInChildren<IDamagable>().Damage(weaponDamage);
                    }
                    if (raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>() != null && !raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
                    {
                        raycastHitDistances.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * 2, ForceMode.Impulse);
                    }
                    gameObjectsHit.Add(raycastHitDistances.ElementAt(i).Value.collider.transform.root.gameObject);
                    remainingPenetrations--;
                }
            }
            //Deal with fire cooldown
            canFire = false;
            Invoke("FinishFireCooldown", fireCooldown);
            //Base fire functionality
            base.Fire();
        }
    }
    public override void Reload()
    {
        if (!isReloading && currentClip != currentClipSize)
        {
            //Do our reload functionality and play sound
            base.Reload();
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundOnObject(gameObject, "AR Reload", false);
            }
        }
    }
    private void FinishFireCooldown()
    {
        canFire = true;
    }
}
