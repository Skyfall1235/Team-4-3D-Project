using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour, ISwitchable, IInteractable
{
    bool isEnabled = false;
    [SerializeField] Animator animator;
    public void Interact()
    {
        if(isEnabled)
        {
            if(animator != null)
            {
                animator.SetTrigger("Open");
            }
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
