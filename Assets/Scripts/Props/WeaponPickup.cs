using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject weaponPlayerInventoryPrefab;
    public void Interact()
    {
        if(GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>() != null)
        {
            Instantiate(weaponPlayerInventoryPrefab, GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().transform.TransformPoint(weaponPlayerInventoryPrefab.GetComponentInChildren<Weapon>().hipFireWeaponPosition), GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().transform.rotation * weaponPlayerInventoryPrefab.transform.rotation, GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().transform);
            GameManager.Instance.currentPlayer.GetComponentInChildren<WeaponManager>().UpdateWeaponInventory();
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
