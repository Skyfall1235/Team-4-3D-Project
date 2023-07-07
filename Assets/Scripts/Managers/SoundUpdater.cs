using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUpdater : MonoBehaviour
{
    public GameObject objectForSound;

    private void Update()
    {
        if (objectForSound != null) 
        {
            transform.position= objectForSound.transform.position;
        }
    }
    private void OnDestroy()
    {
        if (SoundManager.Instance != null)
        {
            if (SoundManager.Instance.playingSounds.ContainsKey(gameObject))
            {
                SoundManager.Instance.playingSounds.Remove(gameObject);
            }
        }
    }
}
