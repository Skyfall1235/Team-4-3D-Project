using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YouDied : MonoBehaviour
{
    [SerializeField]
    public GameObject playButton;
    [SerializeField]
    public GameObject MenuButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

        public void PlayGameB()
        {
            SceneManager.LoadScene("Test Scene");

        }
        public void MainMenuB()
        {
            SceneManager.LoadScene("Main Menu");
        }

}
