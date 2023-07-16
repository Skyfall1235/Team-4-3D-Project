using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HUDManager : MonoBehaviour
{
    //Health TextMeshPro 
    [SerializeField]
    TMP_Text PlayerAmmoInClipAndClipSize; //ammo in clip + clip size
    [SerializeField]
    TMP_Text PlayerReserveAmmo;
    [SerializeField]
    Slider healthSlider;

    void Start()
    {
       // healthSlider.value = 100f;
    }

    void Update()
    {
        

        if (GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<Health>() != null)
        {
            healthSlider.maxValue = GameManager.Instance.currentPlayer.GetComponentInChildren<Health>().currentMaxHealth;
            healthSlider.value = GameManager.Instance.currentPlayer.GetComponentInChildren<Health>().CurrentHealth;
        }

        if(GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>() != null) 
        {
            PlayerReserveAmmo.text = GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().currentWeapon.currentAmmo.ToString();
            PlayerAmmoInClipAndClipSize.text = GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().currentWeapon.currentClip.ToString() + " / " + GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().currentWeapon.currentClipSize.ToString();
        }
    }
}
