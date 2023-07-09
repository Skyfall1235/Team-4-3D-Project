using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    protected int _currentHealth;
    protected int _baseMaxHealth;
    protected int _currentMaxHealth;
    protected bool _invulnerable;
    protected float _invulnerabilityTimeAfterHit;
    protected bool deathTriggered = false;
    protected bool _isDead;
    protected void SetHealthVars(int currentHealth, int maxHealth, bool invulnerable, float invulnerabilityTimeAfterHit)
    {
        _currentHealth = currentHealth;
        _baseMaxHealth = _currentMaxHealth = maxHealth;
        _invulnerable = invulnerable;
        _invulnerabilityTimeAfterHit = invulnerabilityTimeAfterHit;
    }
    public int BaseMaxHealth { get { return _baseMaxHealth; } set { _baseMaxHealth = value; } }
    public bool IsDead
    { get { return _isDead; } }
    public float InvulnerabilityTimerAfterHit
    { get { return _invulnerabilityTimeAfterHit; } set { _invulnerabilityTimeAfterHit = value; ValidateInvulnerabilityTimeAfterHit(); } }
    public bool Invulnerable
    { get { return _invulnerable; } set { _invulnerable = value; } }
    public int CurrentHealth
    { get { return _currentHealth; } set { _currentHealth = value; ValidateHealth(); } }
    public int currentMaxHealth
    { get { return _currentMaxHealth; } set { _currentMaxHealth = value; ValidateHealth(); } }
    //method allows external sources to damage the unit
    public void Damage(int damageAmount)
    {
        if (!_invulnerable)
        {
            if (_currentHealth > 0)
            {
                Mathf.Clamp(_currentHealth -= damageAmount, 0f, _currentHealth);
                OnDamaged();
            }
            ValidateHealth();
            if (_currentHealth > 0 && InvulnerabilityTimerAfterHit > 0)
            {
                StartCoroutine(InvulnerabilityAfterHit(_invulnerabilityTimeAfterHit));
            }
        }
    }
    //method allows external sources to heal the unit
    public void Heal(int healAmount)
    {
        if (_currentHealth < _currentMaxHealth)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + healAmount, _currentHealth, currentMaxHealth);
            OnHealed();
            ValidateHealth();
        }
    }
    //method allows external sources to heal the unit to its maximum health
    protected void MaxHeal()
    {
        if (_currentHealth < _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
            OnHealed();
            ValidateHealth();
        }
    }
    //Method allows External sources to toggle invulnerability
    protected void ToggleInvulnerability()
    {
        if (_invulnerable == true)
        {
            OnInvulnerabilityStopped();
            _invulnerable = false;
        }
        else
        {
            OnInvulnerabilityStarted();
            _invulnerable = true;
        }
    }
    //virtual methods that can be overidden and are called based on things happening in the health script
    public virtual void OnDeath()
    {

    }
    public virtual void OnInvulnerabilityStarted()
    {

    }
    public virtual void OnInvulnerabilityStopped()
    {

    }
    public virtual void OnDamaged()
    {

    }
    public virtual void OnHealed()
    {

    }

    //make sure the health is within specified ranges after any changes
    protected void ValidateHealth()
    {
        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
            deathTriggered = false;
        }
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            if (!deathTriggered)
            {
                OnDeath();
                _isDead = true;
                deathTriggered = true;
            }
        }
        else
        {
            _isDead = false;
            deathTriggered = false;
        }
    }
    //make sure the invulnerability time after hit is within specified ranges
    protected void ValidateInvulnerabilityTimeAfterHit()
    {
        if (_invulnerabilityTimeAfterHit < 0)
        {
            _invulnerabilityTimeAfterHit = 0;
        }
    }
    //coroutine defining the invulnerability sequence
    protected IEnumerator InvulnerabilityAfterHit(float invulnerabilityTimer)
    {
        OnInvulnerabilityStarted();
        _invulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTimer);
        _invulnerable = false;
        OnInvulnerabilityStopped();
    }

}
