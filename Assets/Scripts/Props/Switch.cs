using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable, IResetable
{
    [SerializeField] List<GameObject> objectsToSwitch;
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
            foreach(GameObject go in objectsToSwitch)
            {
                if(go.GetComponent<ISwitchable>() != null)
                {
                    go.GetComponent<ISwitchable>().SwitchedOn();
                }
            }
            
        }
        else
        {
            foreach (GameObject go in objectsToSwitch)
            {
                if (go.GetComponent<ISwitchable>() != null)
                {
                    go.GetComponent<ISwitchable>().SwitchedOff();
                }
            }
        }
    }
}
