using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("General Weapon Options")]
    public bool canADS = true;
     public bool IsAutomatic { get { return _isAutomatic; }}
    [SerializeField] protected bool _isAutomatic;
    [SerializeField] protected float fireCooldown;
    [SerializeField] protected int weaponPenetrationPower = 1;
    [SerializeField] protected float maxFireDistance = 1200f;
    [SerializeField] protected LayerMask bulletLayerMask;
    [SerializeField] protected int weaponDamage = 20;

    //Current States
    public bool isReloading { get; private set; }
    public bool isADS { get; private set; }

    [Header("Ammo Options")]
    public int baseMaxAmmo;
    [HideInInspector] public int currentMaxAmmo;
    protected int currentAmmo;
    public int baseClipSize;
    [HideInInspector] public int currentClipSize;
    protected int currentClip;
    public float baseReloadTime;
    [HideInInspector] public float currentReloadTime;

    [Header("Recoil Options")]
    public Vector3 maxHipFireRecoilAmounts;
    public Vector3 maxADSRecoilAmounts;
    [SerializeField] protected float recoilSnappiness;
    [SerializeField] protected float recoilReturnSpeed;

    [Header("ADS Options")]
    public float adsTransitionTime;
    [SerializeField] protected Vector3 hipFireWeaponPosition;
    [SerializeField] protected Vector3 adsWeaponPosition;
    private IEnumerator currentADSCoroutine;

    [Header("Accuracy Options")]
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
            recoilHandler.HandleRecoilReturn(recoilReturnSpeed, recoilSnappiness);
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
        currentClip = Mathf.Clamp(currentClip - 1, 0, currentClipSize);
        Debug.Log("Clip Remaining: " + currentClip + "/" + currentClipSize);
        //Apply any recoil to the handler
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
        //reload if the clip is empty
        if(currentClip == 0 && !isReloading)
        {
            Reload();
        }
    }
    public virtual void Reload()
    {
        //Invoke the reload functionality after the reload time isover
        isReloading= true;
        Debug.Log("Reloading...");
        Invoke("ReloadFunctionality", currentReloadTime);
    }
    private void ReloadFunctionality()
    {
        //make sure we have some ammo
        if (currentAmmo > 0)
        {
            //if the ammo in the stockpile is less than our clipsize then add the remaining stockpile to the clip
            if (currentAmmo >= currentClipSize)
            {
                int remainingClip = currentClipSize - currentClip;
                currentClip = currentClipSize;
                currentAmmo -= remainingClip;
            }
            //if the ammo in the stockpile is greater than our clipsize then refill the whole thing
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
        //Set our target position depending on our desired state
        Vector3 targetPosition = desiredState ? adsWeaponPosition : hipFireWeaponPosition;
        float adsSpeed = Vector3.Distance(hipFireWeaponPosition, adsWeaponPosition) / adsTransitionTime;
        //Move towards determined target position 
        while(transform.localPosition != targetPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, adsSpeed * Time.deltaTime);
            yield return null;
        }
        //Change our ADS state depending on where we moved to
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