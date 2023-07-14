using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    [SerializeField]
    public GameObject playButton;
    [SerializeField]
    public GameObject settingstButton;
    [SerializeField]
    public GameObject helpButton;
    [SerializeField]
    public GameObject quitButton;
    [SerializeField]
    public CanvasGroup controllerCanvasGroup;
    [SerializeField]
    public Button ExitfromControllerbutton;
    [SerializeField]
    public CanvasGroup volumeCanvasGroup;



    //fading
    [SerializeField]
    private CanvasGroup myUIGroup;
    [SerializeField]
    private bool fadeIn = false;
    [SerializeField]
    private bool fadeOut = false;

    public void ShowUI()
    {
        myUIGroup.alpha = 1;
        fadeIn = true;
    }

    public void HideUI()
    {
        myUIGroup.alpha = 0;
        fadeOut = true;
    }


    private void Start()
    {
        controllerCanvasGroup.alpha = 0;
        volumeCanvasGroup.alpha = 0;
    }

    public void ControllerButtonPress()
    {
        controllerCanvasGroup.alpha = 0;
        Debug.Log("button pressed");
    }

    public void VolumeButtonPress()
    {
        volumeCanvasGroup.alpha = 0;
        Debug.Log("button pressed");
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime;
                if (myUIGroup.alpha >= 1) 
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (myUIGroup.alpha >= 0)
            {
                myUIGroup.alpha += Time.deltaTime;
                if (myUIGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    public void PlayGameB()
    {
        SceneManager.LoadScene("Test Scene");

    }

    public void SettingsB()
    {
        controllerCanvasGroup.alpha = 1;

    }

    public void HelpB()
    {
        volumeCanvasGroup.alpha = 1;

    }

    public void QuitGameB()
    {
        Application.Quit();
    }
}
