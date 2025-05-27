using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Controller : MonoBehaviour
{
    public int DamageAmount = 10;
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<HealthManager>();
            if (health != null)
            {
                health.ApplyDamage(DamageAmount);
            }
        }
    }

}
