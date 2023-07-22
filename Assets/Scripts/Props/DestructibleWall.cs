using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamagable, IResetable
{
    Dictionary<GameObject, Vector3> partStartPositions = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, Vector3> partStartRotations = new Dictionary<GameObject, Vector3>();
    public void Start()
    {
        foreach(Transform child in transform)
        {
            partStartPositions.Add(child.gameObject, child.transform.position);
            partStartRotations.Add(child.gameObject, child.transform.rotation.eulerAngles);
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
        foreach (KeyValuePair<GameObject, Vector3> keyValuePair in partStartRotations)
        {
            keyValuePair.Key.transform.rotation = Quaternion.Euler(keyValuePair.Value);
        }
    }
}
