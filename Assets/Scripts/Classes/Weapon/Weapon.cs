using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //Weapon Options
    public bool canADS = true;
    [SerializeField]
    protected bool isAutomatic;

    //Current States
    protected bool isReloading;
    protected bool isADS;

    //Ammo Related Variables

    public int baseMaxAmmo;
    [HideInInspector]
    public int currentMaxAmmo;

    protected int currentAmmo;

    public int baseClipSize;
    [HideInInspector]
    public int currentClipSize;

    protected int currentClip;

    public float baseReloadTime;
    [HideInInspector]
    public float currentReloadTime;

    //Recoil Related Variables
    public Vector3 maxHipFireRecoilAmounts;
    public Vector3 maxADSRecoilAmounts;
    [SerializeField]
    protected float recoilSnappiness;
    [SerializeField]
    protected float recoilReturnSpeed;


    //ADS Related Variables
    public float adsTransitionTime;
    [SerializeField]
    protected Vector3 hipFireWeaponPosition;
    [SerializeField]
    protected Vector3 adsWeaponPosition;
    private IEnumerator currentADSCoroutine;

    //Accuracy Related Variables
    public Vector2 maxHipFireWeaponInaccuracy;
    public Vector2 maxADSWeaponInaccuracy;

    protected RecoilHandler recoilHandler;

    private void Start()
    {
        currentMaxAmmo = baseMaxAmmo;
        currentAmmo = currentMaxAmmo;
        currentClipSize = baseClipSize;
        currentClip = currentClipSize;
        currentReloadTime = baseReloadTime;
        if(recoilHandler == null && GameManager.Instance != null && GameManager.Instance.currentPlayer != null && GameManager.Instance.currentPlayer.GetComponentInChildren<RecoilHandler>() != null)
        {
            recoilHandler = GameManager.Instance.currentPlayer.GetComponentInChildren<RecoilHandler>();
        }
    }
    private void Update()
    {
        if(recoilHandler!= null)
        {
            recoilHandler.HandleRecoil(recoilReturnSpeed, recoilSnappiness);
        }
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
    public virtual void Fire()
    {
        if(currentClip > 0 && !isReloading)
        {
            currentClip = Mathf.Clamp(currentClip - 1, 0, currentClipSize);
            Debug.Log("Clip Remaining: " + currentClip + "/" + currentClipSize);
            if(recoilHandler!= null)
            {
                if(isADS)
                {
                    recoilHandler.RecoilFire(maxADSRecoilAmounts);
                }
                else
                {
                    recoilHandler.RecoilFire(maxHipFireRecoilAmounts);
                }
            }
            if(currentClip == 0 && !isReloading)
            {
                Reload();
            }
        }
    }
    public virtual void Reload()
    {
        isReloading= true;
        Debug.Log("Reloading...");
        Invoke("ReloadFunctionality", baseReloadTime);
    }
    private void ReloadFunctionality()
    {
        if (currentAmmo > 0)
        {
            if (currentAmmo >= currentClipSize)
            {
                int remainingClip = currentClipSize - currentClip;
                currentClip = currentClipSize;
                currentAmmo -= remainingClip;
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