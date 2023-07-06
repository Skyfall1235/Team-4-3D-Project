using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //Weapon Options
    public bool canADS = true;
    protected bool isAutomatic;

    //Current States
    protected bool isReloading;
    protected bool isADS;

    //Ammo Related Variables
    protected int maxAmmo;
    protected int currentAmmo;
    protected int clipSize;
    protected int currentClip;
    protected float reloadTime;


    //Recoil Related Variables
    protected Vector2 maxHipFireRecoilAmounts;
    protected Vector2 maxADSRecoilAmounts;

    //ADS Related Variables
    protected float adsTransitionTime;
    protected Vector3 hipFireWeaponPosition;
    protected Vector3 adsWeaponPosition;
    private IEnumerator currentADSCoroutine;

    //Accuracy Related Variables
    protected Vector2 maxHipFireWeaponInaccuracy;
    protected Vector2 maxADSWeaponInaccuracy;

    private void Start()
    {
        currentClip = clipSize;
        currentAmmo = maxAmmo;
    }

    public void ChangeADS(bool desiredState)
    {
        if (canADS)
        {
            if (currentADSCoroutine != null)
            {
                StopCoroutine(currentADSCoroutine);
                currentADSCoroutine = ChangeADSState(desiredState);
                StartCoroutine(currentADSCoroutine);
            }
            else
            {
                currentADSCoroutine = ChangeADSState(desiredState);
                StartCoroutine(currentADSCoroutine);
            }
        }
    }
    public void Fire()
    {
        if(currentClip > 0)
        {
            currentClip = Mathf.Clamp(currentClip - 1, 0, clipSize);
            Debug.Log(currentClip);
            if(currentClip == 0 && !isReloading)
            {
                Reload();
            }
        }
    }
    public void Reload()
    {
        isReloading= true;
        Debug.Log("Reloading...");
        Invoke("ReloadFunctionality", reloadTime);
    }
    private void ReloadFunctionality()
    {
        if (currentAmmo > 0)
        {
            if (currentAmmo >= clipSize)
            {
                currentClip = clipSize;
                currentAmmo -= clipSize;
            }
            else
            {
                currentClip = currentAmmo;
                currentAmmo = 0;
            }
        }
        isReloading= false;
        Debug.Log("Reloaded");
    }
    private IEnumerator ChangeADSState(bool desiredState)
    {
        Vector3 targetPosition = desiredState ? adsWeaponPosition : hipFireWeaponPosition;
        float adsSpeed = Vector3.Distance(hipFireWeaponPosition, adsWeaponPosition) / adsTransitionTime;

        while(transform.localPosition != targetPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, adsSpeed * Time.deltaTime);
            yield return null;
        }

        if(transform.localPosition == adsWeaponPosition)
        {
            isADS = true;
        }
        if(transform.localPosition == hipFireWeaponPosition)
        {
            isADS = false;
        }
    }
}