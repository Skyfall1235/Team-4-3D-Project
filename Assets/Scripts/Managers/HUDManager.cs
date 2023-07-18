using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    //Health TextMeshPro 
    [SerializeField]
    TMP_Text PlayerAmmoInClipAndClipSize; //ammo in clip + clip size
    [SerializeField]
    TMP_Text PlayerReserveAmmo;
    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    public CanvasGroup HUDGuidanceText;
    [SerializeField]
    public TMP_Text GuidanceText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            //If we are the Instance, Try to get a valid reference to the current player
            Instance = this;
        }
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
