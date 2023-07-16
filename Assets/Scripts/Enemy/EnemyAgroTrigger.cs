using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAgroTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.Instance != null && other.gameObject == GameManager.Instance.playerCharacterTransform.gameObject)
        {
            BoxCollider[] triggers = GetComponents<BoxCollider>();
            List<GameObject> enemiesInTrigger = new List<GameObject>();

            foreach (BoxCollider trigger in triggers)
            {
                Vector3 offset = (transform.right * trigger.center.x) + (transform.up * trigger.center.y) + (transform.forward * trigger.center.z);
                Vector3 bounds = trigger.bounds.extents;

                Collider[] collidersInTrigger = Physics.OverlapBox(transform.position + offset, bounds, Quaternion.identity);
                foreach (Collider collider in collidersInTrigger)
                {
                    if (collider.gameObject.GetComponentInParent<TestEnemy>() != null && !enemiesInTrigger.Contains(collider.gameObject.GetComponentInParent<TestEnemy>().gameObject))
                    {
                        enemiesInTrigger.Add(collider.gameObject.GetComponentInParent<TestEnemy>().gameObject);
                    }
                }
            }
            foreach (GameObject enemy in enemiesInTrigger)
            {
                enemy.GetComponent<TestEnemy>().isAgroOnPlayer = true;
            }
        }
    }
}
