using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))]
public class TestEnemy : Health, IResetable
{
    [Header("Health")]
    [SerializeField] int startingHealth;
    [SerializeField] int startingMaxHealth;
    [SerializeField] bool startingInvulnerableState;
    [SerializeField] float startingInvulnerabilityTimeAfterHit;

    [Header("Attack")]
    [SerializeField] float attackStartRange = 3f;
    [SerializeField] List<Collider> DamagingColliders = new List<Collider>();
    [SerializeField] int attackDamage = 20;

    //Private Variables
    Vector3 startingPosition;
    Quaternion startingRotation;
    NavMeshAgent agent;
    Animator animator;
    Rigidbody[] rigidbodies;
    Renderer[] renderers;
    Collider[] colliders;
    bool hasRagdolled = false;
    bool hasBeenRemovedFromScene = false;

    void Awake()
    {
        OnValidate();
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
        if(GetComponentsInChildren<Rigidbody>() != null)
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
        }
        if(GetComponentsInChildren<Renderer>() != null)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
        if(GetComponentsInChildren<Collider>() != null)
        {
            colliders = GetComponentsInChildren<Collider>();
        }
        startingPosition= transform.position;
        startingRotation= transform.rotation;
    }
    private void Update()
    {
        if (animator != null)
        {
            animator.SetFloat("Move Speed", new Vector3(agent.velocity.x, 0, agent.velocity.z).magnitude);
            if (GameManager.Instance.currentPlayer != null && Vector3.Distance(transform.position, GameManager.Instance.playerCharacterTransform.position) < attackStartRange && !IsDead)
            {
                animator.SetInteger("Attack Index", Random.Range(0, 2));
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
        if (!IsDead && GameManager.Instance.currentPlayer != null)
        {
            agent.isStopped = false;
            agent.SetDestination(GameManager.Instance.playerCharacterTransform.position);
        }
        else
        {
            agent.isStopped = true;
        }
        if(hasRagdolled && !IsVisibleOnScreen() && !hasBeenRemovedFromScene)
        {
            foreach(Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
            foreach(Rigidbody rb in rigidbodies)
            {
                rb.velocity= Vector3.zero;
                rb.isKinematic = true;
            }
            foreach(Collider coll in colliders)
            {
                coll.enabled = false;
            }
            hasBeenRemovedFromScene= true;
        }
    }
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetHealthVars(startingHealth, startingMaxHealth, startingInvulnerableState, startingInvulnerabilityTimeAfterHit);
        }
    }
    public override void OnDamaged()
    {
        Debug.Log(CurrentHealth.ToString());
    }
    public override void OnDeath()
    {
        animator.SetBool("Dead", true);
    }
    //This has to be called by collision assistant because the child collision events aren't handled by OnCollisionEnter in this script
    public void CollisionDetected(Collision collision)
    {
        if (!IsDead)
        {
            List<GameObject> damagedGameObjects = new List<GameObject>();
            foreach (ContactPoint contact in collision.contacts)
            {
                if (!damagedGameObjects.Contains(contact.otherCollider.transform.root.gameObject) && contact.otherCollider.transform.root.gameObject.GetComponentInChildren<IDamagable>() != null && DamagingColliders.Contains(contact.thisCollider))
                {
                    contact.otherCollider.transform.root.gameObject.GetComponentInChildren<IDamagable>().Damage(attackDamage);
                    damagedGameObjects.Add(contact.otherCollider.transform.root.gameObject);
                }
            }
        }
    }
    public void PostDeathAnimationFunctionaility()
    {
        foreach(Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
        hasRagdolled = true;
        animator.enabled = false;
    }
    public void ResetObject()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
        foreach (Collider coll in colliders)
        {
            coll.enabled = true;
        }
        animator.enabled = true;
        animator.SetBool("Dead", false);
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        MaxHeal();
        hasBeenRemovedFromScene= false;
    }
    private bool IsVisibleOnScreen()
    {
        bool anyRendererIsVisible = false;
        foreach(Renderer renderer in renderers)
        {
            if (renderer.isVisible)
            {
                anyRendererIsVisible = true;
            }
        }
        if (anyRendererIsVisible)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
