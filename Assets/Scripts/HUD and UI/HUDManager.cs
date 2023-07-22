using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    CanvasGroup PauseMenuCanvasGroup;
    [SerializeField]
    CanvasGroup controllerCanvasGroup;
    [SerializeField]
    CanvasGroup UpgradePanelCanvasGroup;

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

        PauseMenuCanvasGroup.alpha = 0.0f;
        controllerCanvasGroup.alpha = 0.0f;
        UpgradePanelCanvasGroup.alpha = 0.0f;
    }

    public void ResumeButtonPressed()
    {


        if (GameManager.Instance != null)
        {
            GameManager.Instance.isPaused = false;
            PauseMenuCanvasGroup.alpha = 0.0f;
            Debug.Log("test");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
        }
       
    }

    public void ControllerButtonPress()
    {
        controllerCanvasGroup.alpha = 1.0f;
        Debug.Log("button pressed");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void ControllerButtonPressEscape()
    {
        controllerCanvasGroup.alpha = 0;
        Debug.Log("button pressed");
    }

    public void UpgradePanelCanvasGroupPressEscape() 
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isPaused = false;
            UpgradePanelCanvasGroup.alpha = 0.0f;
            Debug.Log("test");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
        //UpgradePanelCanvasGroup.blocksRaycasts = false;
    }

    public void MainMenuB()
    {
        //FindAnyObjectByType<SceneTransitionAnimationHandler>().StartTransition("Main Menu scene");
        SceneManager.LoadScene("Main Menu scene");

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
        if (Input.GetKeyUp(KeyCode.Escape) && GameManager.Instance != null)
        {
            PauseMenuCanvasGroup.alpha = 1.0f;
            GameManager.Instance.isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //blocking raycast
            UpgradePanelCanvasGroup.blocksRaycasts = false;
            PauseMenuCanvasGroup.blocksRaycasts = true;

        }

        if (Input.GetKeyUp(KeyCode.Tab) && GameManager.Instance != null)
        {
            UpgradePanelCanvasGroup.alpha = 1.0f;
            GameManager.Instance.isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //blocking raycast
            UpgradePanelCanvasGroup.blocksRaycasts = true;
            PauseMenuCanvasGroup.blocksRaycasts = false;



        }

    }
}
