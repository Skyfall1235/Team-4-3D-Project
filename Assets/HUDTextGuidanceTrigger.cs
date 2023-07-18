using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HUDTextGuidanceTrigger : MonoBehaviour
{
    [SerializeField]
    string Guidance;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance != null && GameManager.Instance.currentPlayer != null && other.gameObject == GameManager.Instance.playerCharacterTransform.gameObject)
        { 
            if(HUDManager.Instance != null) 
            {
                HUDManager.Instance.HUDGuidanceText.alpha = 1.0f;
                HUDManager.Instance.GuidanceText.text = Guidance;
                
            }
        }
    }

    private void OnTriggerExit(Collider other)

    {
        if (GameManager.Instance != null && GameManager.Instance.currentPlayer != null && other.gameObject == GameManager.Instance.playerCharacterTransform.gameObject)
        {
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.HUDGuidanceText.alpha = 0.0f;

            }
        }
    }
}
