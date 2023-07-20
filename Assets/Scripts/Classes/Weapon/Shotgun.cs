using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Weapon Specific Options")]
    [SerializeField] int projectileCount;
  
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
                SoundManager.Instance.PlaySoundOnObject(gameObject, "Shotgun Shot", false);
            }
            //create a dictionary of dictionaries  
            Dictionary<int, SortedDictionary<float, RaycastHit>> projectileRaycastHits = new Dictionary<int, SortedDictionary<float, RaycastHit>>();
            for (int i = 0; i < projectileCount; i++)
            {
                //for every shot we need to shoot, calculate a shot deviation
                Vector3 hipFireShotDeviation = (playerCam.gameObject.transform.up * Random.Range(-maxHipFireWeaponInaccuracy.x, maxHipFireWeaponInaccuracy.x)) + (playerCam.gameObject.transform.right * Random.Range(-maxHipFireWeaponInaccuracy.y, maxHipFireWeaponInaccuracy.y));
                Vector3 adsShotdeviation = (playerCam.transform.up * Random.Range(-maxADSWeaponInaccuracy.x, maxADSWeaponInaccuracy.x)) + (playerCam.transform.right * Random.Range(-maxADSWeaponInaccuracy.y, maxADSWeaponInaccuracy.y));
                //fire the raycast for the shot
                RaycastHit[] bulletHits = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward + (isADS ? adsShotdeviation : hipFireShotDeviation), maxFireDistance, bulletLayerMask, QueryTriggerInteraction.Ignore);

                //Create a sorted dictionary to store our raycast hit and their respective distances in
                SortedDictionary<float, RaycastHit> raycastHitDistances = new SortedDictionary<float, RaycastHit>();
                //Add the respective vaues to the dictionary
                foreach (RaycastHit hit in bulletHits)
                {
                    if (!raycastHitDistances.ContainsKey(Vector3.Distance(playerCam.transform.position, hit.point)))
                    {
                        raycastHitDistances.Add(Vector3.Distance(playerCam.transform.position, hit.point), hit);
                    }
                }
                projectileRaycastHits.Add(i, raycastHitDistances);
            }
            //For every shot we fired, loop through the hits and decrease our penetration value for everything we come into contact with
            foreach(KeyValuePair<int, SortedDictionary<float, RaycastHit>> projectileDictionary in projectileRaycastHits)
            {
                List<GameObject> gameObjectsHit = new List<GameObject>();
                remainingPenetrations = weaponPenetrationPower;
                for (int i = 0; i < projectileDictionary.Value.Count && remainingPenetrations > 0; i++)
                {
                    if (!gameObjectsHit.Contains(projectileDictionary.Value.ElementAt(i).Value.collider.transform.root.gameObject))
                    {
                        if (projectileDictionary.Value.ElementAt(i).Value.collider.gameObject.transform.root.GetComponentInChildren<IDamagable>() != null)
                        {
                            projectileDictionary.Value.ElementAt(i).Value.collider.gameObject.transform.root.GetComponentInChildren<IDamagable>().Damage(weaponDamage);
                        }
                        if (projectileDictionary.Value.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>() != null && !projectileDictionary.Value.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
                        {
                            projectileDictionary.Value.ElementAt(i).Value.collider.gameObject.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * 2, ForceMode.Impulse);
                        }
                        gameObjectsHit.Add(projectileDictionary.Value.ElementAt(i).Value.collider.transform.root.gameObject);
                        remainingPenetrations--;
                    }
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
                SoundManager.Instance.PlaySoundOnObject(gameObject, "Shotgun Reload", false);
            }
        }
    }
    private void Awake()
    {
        AquirePlayerCam();
    }
    private void FinishFireCooldown()
    {
        canFire = true;
    }
}
