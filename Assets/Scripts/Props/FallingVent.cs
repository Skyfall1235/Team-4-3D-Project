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

        rb = GetComponent<Rigidbody>();
        startPosition= transform.position;
    }
    public void Damage(int damage) 
    {
        rb.isKinematic = false;
        rb.AddForce(Vector3.zero);
    }
    public void ResetObject()
    {
        rb.isKinematic = true;
        transform.position = startPosition;
    }
}
