using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionDataHandler : MonoBehaviour
{
    int maxHealthUpgrades;
    [SerializeField] int initialMaxHealthUpgrades = 0;
    int clipSizeUpgrades;
    [SerializeField] int initialClipSizeUpgrades = 0;
    int maxAmmoUpgrades;
    [SerializeField] int initialMaxAmmoUpgrades = 0;
    int reloadSpeedUpgrades;
    [SerializeField] int initialReloadSpeedUpgrades = 0;
    int playerMoveSpeedUpgrades;
    [SerializeField] int initialPlayerMoveSpeedUpgrades = 0;

    List<GameObject> playerWeaponInventory;
    [SerializeField]List<GameObject> initialPlayerWeaponInventory;

    public static TransitionDataHandler Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
   
    }
    void OnEnable()
    {
        //Tell our function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    void OnDisable()
    {
        //Tell our function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnSceneChanged;
    }
    public void UpdateData()
    {
        if(UpgradeManager.Instance != null)
        {
            maxHealthUpgrades = UpgradeManager.Instance.maxHealthUpgrades;
            clipSizeUpgrades = UpgradeManager.Instance.clipSizeUpgrades;
            maxAmmoUpgrades = UpgradeManager.Instance.maxAmmoUpgrades;
            reloadSpeedUpgrades = UpgradeManager.Instance.reloadSpeedUpgrades;
            playerMoveSpeedUpgrades = UpgradeManager.Instance.playerMoveSpeedUpgrades;
        }
        if(GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>() != null) 
        {
            playerWeaponInventory.Clear();
            foreach (Weapon weapon in GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().weaponInventory)
            {
                playerWeaponInventory.Add(weapon.gameObject);
            }
            
        }
    }

    public void LoadStoredData()
    {
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.maxHealthUpgrades = maxHealthUpgrades;
            UpgradeManager.Instance.clipSizeUpgrades= clipSizeUpgrades;
            UpgradeManager.Instance.maxAmmoUpgrades= maxAmmoUpgrades;
            UpgradeManager.Instance.reloadSpeedUpgrades = reloadSpeedUpgrades;
            UpgradeManager.Instance.playerMoveSpeedUpgrades= playerMoveSpeedUpgrades;
            UpgradeManager.Instance.ApplyUpgrades();
        }
        if (GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>() != null)
        {
            foreach (GameObject go in playerWeaponInventory)
            {
                Instantiate(go, GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().transform.TransformPoint(go.GetComponentInChildren<Weapon>().hipFireWeaponPosition), GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().transform.rotation * go.transform.rotation,  GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().transform);
            }
            GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().UpdateWeaponInventory();
        }
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Level One scene")
        {
            maxHealthUpgrades = initialMaxHealthUpgrades;
            clipSizeUpgrades= initialClipSizeUpgrades;
            maxAmmoUpgrades= initialMaxAmmoUpgrades;
            reloadSpeedUpgrades= initialReloadSpeedUpgrades;
            playerMoveSpeedUpgrades= initialPlayerMoveSpeedUpgrades;
            playerWeaponInventory = new List<GameObject>(initialPlayerWeaponInventory);
            LoadStoredData();
        }
        else
        {
            LoadStoredData();
        }
    }
}
