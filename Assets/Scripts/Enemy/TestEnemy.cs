using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))]
public class TestEnemy : Health
{
    [Header("Health")]
    [SerializeField] int startingHealth;
    [SerializeField] int startingMaxHealth;
    [SerializeField] bool startingInvulnerableState;
    [SerializeField] float startingInvulnerabilityTimeAfterHit;

    [Header("Attack")]
    [SerializeField] float attackStartRange = 3f;
    [SerializeField] List<Collider> DamagingColliders= new List<Collider>();
    [SerializeField] int attackDamage = 20;

    //Private Variables
    NavMeshAgent agent;
    Animator animator;

    void Awake()
    {
        OnValidate();
        if(GetComponent<NavMeshAgent>() != null )
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if(GetComponent<Animator>() != null )
        {
            animator = GetComponent<Animator>();
        }
    }
    private void Update()
    {
        if(animator != null)
        {
            animator.SetFloat("Move Speed", new Vector3(agent.velocity.x, 0, agent.velocity.z).magnitude);
            if(Vector3.Distance(transform.position, GameManager.Instance.playerCharacterTransform.position) < attackStartRange && !IsDead)
            {
                animator.SetInteger("Attack Index", Random.Range(0, 2));
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
        if (!IsDead)
        {
            agent.SetDestination(GameManager.Instance.playerCharacterTransform.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetHealthVars(startingHealth, startingMaxHealth, startingInvulnerableState, startingInvulnerabilityTimeAfterHit);
        }

        Collider[] collidersInEnemy = GetComponentsInChildren<Collider>();

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
        List<GameObject> damagedGameObjects = new List<GameObject>();
        foreach(ContactPoint contact in collision.contacts)
        {
            if (!damagedGameObjects.Contains(contact.otherCollider.transform.root.gameObject) && contact.otherCollider.transform.root.gameObject.GetComponentInChildren<IDamagable>() != null && DamagingColliders.Contains(contact.thisCollider))
            {
                contact.otherCollider.transform.root.gameObject.GetComponentInChildren<IDamagable>().Damage(attackDamage);
                damagedGameObjects.Add(contact.otherCollider.transform.root.gameObject);
            }
        }
    }
    public void PostDeathAnimationFunctionaility()
    {
        Destroy(gameObject);
    }
}
