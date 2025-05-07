using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ShotgunPellet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;
    public int damage = 10;
    public LayerMask hitLayers;
    public ParticleSystem impactEffect;
    public GameObject bloodBurstEffectPrefab;
    public GameObject bulletTrailPrefab;

    private Rigidbody rb;
    private GameObject bulletTrailInstance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = -transform.forward * speed;

        // Instanciar el rastro de la bala
        if (bulletTrailPrefab)
        {
            bulletTrailInstance = Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);
            bulletTrailInstance.transform.SetParent(transform); // Que siga la bala
        }

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0]; // Primer punto de contacto

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (bloodBurstEffectPrefab)
            {
                GameObject burst = Instantiate(bloodBurstEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
                ParticleSystem burstParticles = burst.GetComponent<ParticleSystem>();
                if (burstParticles != null)
                {
                    burstParticles.Play();
                }
                Destroy(burst, 3f);
            }

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            else
            {
                RangeEnemy rangeEnemy = collision.gameObject.GetComponent<RangeEnemy>();
                if (rangeEnemy != null)
                {
                    rangeEnemy.TakeDamage(damage);
                }
            }

        }

        if (((1 << collision.gameObject.layer) & hitLayers) != 0)
        {

            if (impactEffect)
            {
                ParticleSystem impact = Instantiate(impactEffect, contact.point, Quaternion.LookRotation(contact.normal));
                impact.transform.SetParent(collision.transform);
                Destroy(impact.gameObject, 1.5f);
            }

            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if (bulletTrailInstance)
        {
            bulletTrailInstance.transform.SetParent(null);
            Destroy(bulletTrailInstance, 0.5f);
        }

        Destroy(gameObject);
    }
}
