using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer)), RequireComponent(typeof(Collider))]
public class HealthPickup : MonoBehaviour, IResetable
{
    [SerializeField] int healAmount;
    Renderer rend;
    Collider coll;
    void Start()
    {
        if(GetComponent<Renderer>() != null)
        {
            rend = GetComponent<Renderer>();
        }
        if(GetComponent<Collider>() != null)
        {
            coll = GetComponent<Collider>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.GetComponentInChildren<FirstPersonControllerV2>() != null && other.transform.root.GetComponentInChildren<FirstPersonControllerV2>().CurrentHealth < other.transform.root.GetComponentInChildren<FirstPersonControllerV2>().currentMaxHealth)
        {
            other.transform.root.GetComponentInChildren<FirstPersonControllerV2>().Heal(healAmount);
            if(coll!= null)
            {
                coll.enabled = false;
            }
            if(rend != null)
            {
                rend.enabled = false;
            }
        }
    }
    public void ResetObject()
    {
        if (coll != null)
        {
            coll.enabled = true;
        }
        if (rend != null)
        {
            rend.enabled = true;
        }
    }
}
