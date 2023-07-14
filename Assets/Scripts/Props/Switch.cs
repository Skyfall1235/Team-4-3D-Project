using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable, IResetable
{
    [SerializeField] GameObject objectToSwitch;
    [SerializeField] bool defaultSwitchState;
    bool isOn;
    public void Start()
    {
        isOn = defaultSwitchState;
        UpdateSwitchableState();
    }
    public void Interact()
    {
        isOn = !isOn;
        UpdateSwitchableState();
    }
    public void ResetObject()
    {
        isOn = defaultSwitchState;
        UpdateSwitchableState();
    }
    void UpdateSwitchableState()
    {
        if (isOn)
        {
            objectToSwitch.GetComponent<ISwitchable>().SwitchedOn();
        }
        else
        {
            objectToSwitch.GetComponent<ISwitchable>().SwitchedOff();
        }
    }
}
