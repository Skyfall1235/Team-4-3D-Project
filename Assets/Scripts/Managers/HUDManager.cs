using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //Health TextMeshPro 
    [SerializeField]
    TMP_Text PlayerHealthText;
    TMP_Text PlayerAmmoInClipAndClipSize; //ammo in clip + clip size
    TMP_Text PlayerReserveAmmo;


    void Start()
    {

    }

    void Update()
    {

        
        if(GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<Health>() != null)
        {
            PlayerHealthText.text = GameManager.Instance.currentPlayer.GetComponentInChildren<Health>().CurrentHealth.ToString();
        }
    }
}
