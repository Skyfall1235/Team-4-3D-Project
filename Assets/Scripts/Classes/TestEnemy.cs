using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))]
public class TestEnemy : Health
{
    //Inspector Variables
    [SerializeField]
    int startingHealth;
    [SerializeField]
    int startingMaxHealth;
    [SerializeField]
    bool startingInvulnerableState;
    [SerializeField]
    float startingInvulnerabilityTimeAfterHit;

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
    }
    public override void OnDamaged()
    {
        Debug.Log(CurrentHealth.ToString());
    }
    public override void OnDeath()
    {
        animator.SetBool("Dead", true);
    }
    public void PostDeathAnimationFunctionaility()
    {
        Destroy(gameObject);
    }
}
