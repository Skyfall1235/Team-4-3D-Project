using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkingWires : MonoBehaviour, ISwitchable
{
    [SerializeField] int damage;
    GameObject sparkingWireSoundEffect;
    bool isPoweredOn= false;

    private void OnTriggerStay(Collider other)
    {
        if (isPoweredOn && other.gameObject.GetComponentInChildren<Health>() != null)
        {
            other.gameObject.GetComponentInChildren<Health>().Damage(damage);
        }
    }
    public void SwitchedOn()
    {
        isPoweredOn = true;
        ParticleSystem[] sparkParticles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in sparkParticles)
        {
            particle.Play();
        }
        if(SoundManager.Instance != null && sparkingWireSoundEffect == null)
        {
            sparkingWireSoundEffect = SoundManager.Instance.PlaySoundOnObject(gameObject, "Sparking Wires", true);
        }
    }
    public void SwitchedOff()
    {
        isPoweredOn = false;
        ParticleSystem[] sparkParticles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in sparkParticles)
        {
            particle.Stop();
        }
        if (sparkingWireSoundEffect != null)
        {
            Destroy(sparkingWireSoundEffect);
        }
    }
}
