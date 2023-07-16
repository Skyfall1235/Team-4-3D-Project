using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamagable, IResetable
{
    Dictionary<GameObject, Vector3> partStartPositions = new Dictionary<GameObject, Vector3>();
    public void Start()
    {
        foreach(Transform child in transform)
        {
            partStartPositions.Add(child.gameObject, child.transform.position);
        }
    }

    public void Damage(int damage)
    {
        foreach (Transform child in transform)
        {
            if(child.gameObject.GetComponent<Rigidbody>() != null)
            {
                child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
    public void ResetObject()
    {
        foreach(KeyValuePair<GameObject, Vector3> keyValuePair in partStartPositions)
        {
            keyValuePair.Key.transform.position= keyValuePair.Value;
            if(keyValuePair.Key.gameObject.GetComponent<Rigidbody>() != null)
            {
                keyValuePair.Key.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
