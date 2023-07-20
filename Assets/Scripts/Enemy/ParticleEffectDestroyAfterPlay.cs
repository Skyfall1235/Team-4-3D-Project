using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffectDestroyAfterPlay : MonoBehaviour
{
    ParticleSystem ps;
    bool hasStartedPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<ParticleSystem>() != null)
        {
            ps = GetComponent<ParticleSystem>();
        }
        else
        {
            Destroy(gameObject);
        }

        if(ps != null && ps.isStopped)
        {
            ps.Play();
            hasStartedPlaying = true;
        }
        else
        {
            hasStartedPlaying= true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStartedPlaying && ps.isStopped && ps != null)
        {
            Destroy(gameObject);
        }

        if(ps == null)
        {
            Destroy(gameObject);
        }
    }
}
