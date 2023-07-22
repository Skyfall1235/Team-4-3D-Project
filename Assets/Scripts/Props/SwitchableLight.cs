using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableLight : MonoBehaviour, ISwitchable
{
    Light[] lightsToSwitch;
    void Awake()
    {
        if(GetComponentsInChildren<Light>() != null)
        {
            lightsToSwitch = GetComponentsInChildren<Light>();
        }
    }
    public void SwitchedOn()
    {
        if(lightsToSwitch != null)
        {
            foreach (Light light in lightsToSwitch)
            {
                light.enabled = true;
            }
        }
    }
    public void SwitchedOff() 
    {
        if(lightsToSwitch != null)
        {
            foreach (Light light in lightsToSwitch)
            {
                light.enabled = false;
            }
        }
    }
}
