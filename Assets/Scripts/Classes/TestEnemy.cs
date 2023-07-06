using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Health
{
    [SerializeField]
    int startingHealth;
    [SerializeField]
    int startingMaxHealth;
    [SerializeField]
    bool startingInvulnerableState;
    [SerializeField]
    float startingInvulnerabilityTimeAfterHit;
    // Start is called before the first frame update
    void Awake()
    {
        OnValidate();
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
        Destroy(gameObject);
    }
}
