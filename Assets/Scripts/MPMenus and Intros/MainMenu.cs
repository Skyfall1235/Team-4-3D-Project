using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("1");

    }

    public void HelpB()
    {
        SceneManager.LoadScene("1");

    }

    public void QuitGameB()
    {
        Application.Quit();
    }
}
