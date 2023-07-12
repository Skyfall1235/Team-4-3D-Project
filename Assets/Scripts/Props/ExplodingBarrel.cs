using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem)), RequireComponent(typeof(Collider)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(Rigidbody))]
public class ExplodingBarrel : MonoBehaviour, IDamagable, IResetable
{
    //Inspector Vars
    [SerializeField]
    float explosionRadius;
    [SerializeField]
    int explosionDamage;

    //Private Vars
    ParticleSystem[] ps;
    Collider oc;
    MeshRenderer mr;
    Rigidbody rb;
    Vector3 startingPosition;
    Quaternion startingRotation;
    bool hasExploded = false;
    void Start()
    {
        if(GetComponents<ParticleSystem>() != null) 
        {
            ps = GetComponents<ParticleSystem>();
        }
        if(GetComponent<Collider>() != null)
        {
            oc = GetComponent<Collider>();
        }
        if(GetComponent<MeshRenderer>() != null)
        {
            mr = GetComponent<MeshRenderer>();
        }
        if(GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }

        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }
    public void Damage(int Damage)
    {
        if(!hasExploded)
        {
            if (ps != null)
            {
                foreach(ParticleSystem ps in ps)
                {
                    ps.Play();
                }
            }
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            if (oc != null)
            {
                oc.enabled = false;
            }
            if (mr != null)
            {
                mr.enabled = false;
            }
            Collider[] objectsInBlastRange = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider obj in objectsInBlastRange)
            {
                if (obj.gameObject.GetComponent<IDamagable>() != null)
                {
                    obj.gameObject.GetComponent<IDamagable>().Damage(explosionDamage);
                }
            }
            hasExploded = true;
        }
    }
    public void ResetObject()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        oc.enabled = true;
        rb.isKinematic = false;
        mr.enabled = true;
        hasExploded= false;
    }
}
