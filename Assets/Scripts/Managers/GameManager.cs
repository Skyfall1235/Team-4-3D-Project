using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float respawnTime = 2f;
    [SerializeField] Vector3 spawnpoint;
    public static GameManager Instance { get; private set; }
    GameObject[] foundPlayers;
    public GameObject currentPlayer { get; private set; }
    public Transform playerCharacterTransform { get; private set; }
    private void Awake()
    {
        //Manage singleton instance of GameManager
        if(Instance != null && Instance != this)
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
    public void OnPlayerDeath()
    {
        //Invoke("RespawnPlayer", respawnTime);
        //MP adding
        SceneManager.LoadScene("You Died");
    }
    void RespawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, spawnpoint, Quaternion.identity);
        playerCharacterTransform = currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().gameObject.transform;
    }
}
