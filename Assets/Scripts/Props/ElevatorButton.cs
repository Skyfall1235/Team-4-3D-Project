using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour, ISwitchable, IInteractable
{
    bool isEnabled = false;
    public void Interact()
    {
        if(isEnabled)
        {
            Debug.Log("Going Up!");
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundOnObject(gameObject, "ElevatorDing", false);
            }
        }
    }
    public void SwitchedOn()
    {
        isEnabled= true;
    }
    public void SwitchedOff() 
    {
        isEnabled= false;
    }
}
