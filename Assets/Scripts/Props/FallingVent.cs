using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingVent : MonoBehaviour, IResetable, IDamagable
{
    Rigidbody rb;
    Vector3 startPosition;
    void Start()
    {
        if(GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }
        startPosition= transform.position;
    }
    public void Damage(int damage) 
    {
        rb.isKinematic = false;
    }
    public void ResetObject()
    {
        rb.isKinematic = false;
        transform.position = startPosition;
    }
}
