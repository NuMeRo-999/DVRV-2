using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform target;
    public float speed = 30f;
    public float lifetime = 3f;
    public int damage = 10;
    public LayerMask hitLayers;
    public ParticleSystem impactEffect;
    public GameObject bulletTrailPrefab;

    private GameObject bulletTrailInstance;

    private void Start()
    {
        if (bulletTrailPrefab)
        {
            bulletTrailInstance = Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);
            bulletTrailInstance.transform.SetParent(transform);
        }

        Destroy(gameObject, lifetime);
    }

    public void Initialize(Transform player, float bulletSpeed, int bulletDamage, LayerMask layer, float bulletLifetime)
    {
        target = player;
        speed = bulletSpeed;
        damage = bulletDamage;
        lifetime = bulletLifetime;
        
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.forward = direction;
        }
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        if (((1 << other.gameObject.layer) & hitLayers) != 0)
        {
            if (impactEffect)
            {
                ParticleSystem impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
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
