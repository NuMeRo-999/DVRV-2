using UnityEngine;
using UnityEngine.AI;

public class RangeEnemy : MonoBehaviour
{
    public enum EnemyState { Idle, Alert, Attack }
    public Transform player;
    private Animator animator;

    public GameObject meatGenerator;
    public bool isDead = false;
    public float health = 100f;

    public EnemyState currentState = EnemyState.Idle; 
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float detectionRange = 10f;
    public float attackRange = 7f;
    public float moveSpeed = 3f;
    public float fireRate = 1.5f;
    public float searchTime = 2f; // Tiempo que se queda en alerta tras perder al jugador

    private NavMeshAgent agent;
    private float nextFireTime;
    private Vector3 lastKnownPosition;
    private float searchTimer;
    public AudioSource dieSound;
    public AudioSource shootSound;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("Running", false);
                animator.SetBool("Shooting", false);
                agent.isStopped = true;
                if (distanceToPlayer <= detectionRange)
                {
                    lastKnownPosition = player.position; // Guarda la posición del jugador
                    currentState = EnemyState.Alert;
                }
                break;

            case EnemyState.Alert:
                animator.SetBool("Running", true);

                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                else if (distanceToPlayer > detectionRange)
                {
                    searchTimer += Time.deltaTime;
                    agent.isStopped = true; // Se queda quieto en la última posición conocida
                    if (searchTimer >= searchTime)
                    {
                        currentState = EnemyState.Idle; // Si pasa el tiempo, vuelve a Idle
                        searchTimer = 0f;
                    }
                }
                else
                {
                    lastKnownPosition = player.position; // Actualiza la última posición conocida
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
                break;

            case EnemyState.Attack:
                animator.SetBool("Running", false);
                animator.SetBool("Shooting", true);
                agent.isStopped = true;
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

                if (distanceToPlayer > attackRange)
                {
                    currentState = EnemyState.Alert;
                    searchTimer = 0f;
                }
                else if (Time.time >= nextFireTime)
                {
                    Shoot();
                }
                break;
        }
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        if (shootSound) shootSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
        enemyBullet.Initialize(player, 10f, 10, LayerMask.GetMask("Player"), 3f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (meatGenerator && !isDead)
        {
            Instantiate(meatGenerator, transform.position + Vector3.up, Quaternion.identity);
            isDead = true;
            if (dieSound) dieSound.Play();
        }
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
