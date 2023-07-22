using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Switch : MonoBehaviour, IInteractable, IResetable
{
    [SerializeField] List<GameObject> objectsToSwitch;
    [SerializeField] bool defaultSwitchState;
    bool isOn;
    bool canInteract = true;
    Animator animator;
    public void Start()
    {
        if(GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
        isOn = defaultSwitchState;
        UpdateSwitchableState();
    }
    public void Interact()
    {
        if(canInteract) 
        {
            isOn = !isOn;
            if (animator != null)
            {
                animator.SetBool("SwitchState", isOn);
            }
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundOnObject(gameObject, "Switchnoise", false);
            }
            UpdateSwitchableState();
            canInteract= false;
        }
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

    void EnableInteraction()
    {
        canInteract = true;
    }
}
