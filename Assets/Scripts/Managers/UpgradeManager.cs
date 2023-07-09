using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    void UpgradeMaxHealth()
    {
        GameManager.Instance.currentPlayer.GetComponentInChildren<FirstPersonControllerV2>().MaxHealth += 10;
    }
    void UpgradeClipSize()
    {
        foreach(Weapon weapon in GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().weaponInventory)
        {
            weapon.clipSize += 2;
        }
    }
    void UpgradeReloadSpeed()
    {

    }
    void UpgradePlayerMoveSpeed()
    {

    }
}
