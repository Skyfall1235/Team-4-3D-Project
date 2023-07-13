using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractible
{
    public void Interact()
    {
        Debug.Log("Interacted!");
    }
}
