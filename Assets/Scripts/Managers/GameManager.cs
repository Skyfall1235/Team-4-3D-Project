using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float respawnTime = 2f;
    [SerializeField] Vector3 spawnLocation;
    [SerializeField] Vector3 spawnRotation;


    public static GameManager Instance { get; private set; }
    GameObject[] foundPlayers;
    public GameObject currentPlayer { get; private set; }
    public Transform playerCharacterTransform { get; private set; }

    private void Start()
    {
        StartCoroutine(PlayAmbientSoundCoroutine(120.0f, playerCharacterTransform.gameObject, "AmbientLaugh", false));

        //rain sound
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySoundOnObject(gameObject, "RainAmbient", true);
        }
    }

    private void Awake()
    {


        //Manage singleton instance of GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            //If we are the Instance, Try to get a valid reference to the current player
            Instance = this;
            foundPlayers = GameObject.FindGameObjectsWithTag("Player");
            if(foundPlayers.Length <= 0 ) 
            {
                Debug.LogError("No Players Found");
            }
            else if(foundPlayers.Length > 1)
            {
                Debug.LogWarning("More than one player found in scene, using the first one found");
                currentPlayer = foundPlayers[0];
            }
            else
            {
                currentPlayer= foundPlayers[0];
            }
        }
        //Get a reference to the player's position since it wont be the containing object 
        if(currentPlayer != null) 
        {
            playerCharacterTransform = currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().gameObject.transform;
        }
        
    }

    //Method for looping at a time soundsS
    IEnumerator PlayAmbientSoundCoroutine(float timeBetweenPlay, GameObject objectForSound, string nameOfSound, bool loop)
    { 
    yield return new WaitForSeconds(timeBetweenPlay);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySoundOnObject(objectForSound, nameOfSound, loop);
            StartCoroutine(PlayAmbientSoundCoroutine(timeBetweenPlay, objectForSound, nameOfSound, loop));
        }
    }


    public void OnPlayerDeath()
    {
        Invoke("RespawnPlayer", respawnTime);
        var resetables = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>();
        foreach (IResetable resetable in resetables)
        {
            resetable.ResetObject();
        }
    }
    void RespawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, spawnLocation, Quaternion.Euler(spawnRotation));
        playerCharacterTransform = currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().gameObject.transform;
    }
}
