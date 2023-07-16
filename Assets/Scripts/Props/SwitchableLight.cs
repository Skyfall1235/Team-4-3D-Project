using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableLight : MonoBehaviour, ISwitchable
{
    Light[] lightsToSwitch;
    void Start()
    {
        lightsToSwitch = GetComponentsInChildren<Light>();
    }
    public void SwitchedOn()
    {
        foreach (Light light in lightsToSwitch)
        {
            light.enabled = true;
        }
    }
    public void SwitchedOff() 
    {
        foreach (Light light in lightsToSwitch)
        {
            light.enabled = false;
        }
    }
}
