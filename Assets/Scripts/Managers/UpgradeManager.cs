using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int maxHealthUpgradeAmount = 10;
    public int maxHealthUpgrades  = 0;

    public int maxClipSizeUpgradeAmount = 2;
    public int clipSizeUpgrades  = 0;

    public int maxAmmoUpgradeAmount = 50;
    public int maxAmmoUpgrades = 0;

    public float reloadSpeedUpgradeAmount = .8f; 
    public int reloadSpeedUpgrades = 0;

    public float playerMoveSpeedUpgradeAmount = 1f;
    public int playerMoveSpeedUpgrades = 0;

    public static UpgradeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    void UpgradeMaxHealth()
    {
        maxHealthUpgrades++;
        ApplyUpgrades();
    }
    void UpgradeClipSize()
    {
        clipSizeUpgrades++;
        ApplyUpgrades();
    }
    void UpgradeMaxAmmo()
    {
        maxAmmoUpgrades++;
        ApplyUpgrades();
    }
    void UpgradeReloadSpeed()
    {
        reloadSpeedUpgrades++;
        ApplyUpgrades();
    }
    void UpgradePlayerMoveSpeed()
    {
        playerMoveSpeedUpgrades++;
        ApplyUpgrades();
    }
    public void ApplyUpgrades()
    {
        if(GameManager.Instance.currentPlayer != null)
        {
            //Handle health upgrades
            GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().currentMaxHealth = GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().BaseMaxHealth + (maxHealthUpgrades * maxHealthUpgradeAmount);
            //Handle move speed Upgrades
            GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().currentWalkMoveSpeed = GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().baseWalkMoveSpeed + (playerMoveSpeedUpgrades * playerMoveSpeedUpgradeAmount);
            GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().currentSprintMoveSpeed = GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().baseSprintMoveSpeed + (playerMoveSpeedUpgrades * playerMoveSpeedUpgradeAmount);
            GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().currentCrouchMoveSpeed = GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().baseCrouchMoveSpeed + (playerMoveSpeedUpgrades * playerMoveSpeedUpgradeAmount);
            foreach (Weapon weapon in GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().weaponInventory)
            {
                //Handle clip size upgrades
                weapon.currentClipSize = weapon.baseClipSize + (clipSizeUpgrades * maxClipSizeUpgradeAmount);
                //Handle max ammo upgrades
                weapon.currentMaxAmmo = weapon.baseMaxAmmo + (maxAmmoUpgrades * maxAmmoUpgradeAmount);
                //Handle reload speed upgrade
                float newReloadSpeed = weapon.baseReloadTime;
                for (int i = 0; i < reloadSpeedUpgrades; i++)
                {
                    newReloadSpeed *= reloadSpeedUpgrades;
                }
                weapon.currentReloadTime = newReloadSpeed;

            }
        }
    } 
}
