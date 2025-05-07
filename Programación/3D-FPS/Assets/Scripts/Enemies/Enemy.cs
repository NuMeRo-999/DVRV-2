using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public GameObject meatGenerator;
    public bool isDead = false;
    public float health = 100f;

    public enum NPCState { Idle, Alert, Attack }
    public NPCState currentState = NPCState.Idle;

    [Header("Player Tracking")]
    [SerializeField] private float playerDetectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float stopChaseDistance = 1.5f;
    [SerializeField] private float angleVision = 60f;

    [Header("Attack Settings")]
    [SerializeField] private Collider attackCollider;  // Collider del ataque
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    private bool canAttack = true;

    public AudioSource dieSound;
    public AudioSource attackSound;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.isStopped = true;
        navMeshAgent.stoppingDistance = stopChaseDistance;

        attackCollider.enabled = false; // Desactivar el collider al inicio
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Idle:
                DetectPlayer();
                break;
            case NPCState.Alert:
                ChasePlayer();
                break;
            case NPCState.Attack:
                Attack();
                break;
        }
    }

    private void DetectPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;

        if (Vector3.Distance(transform.position, player.transform.position) < playerDetectionRange &&
            Vector3.Angle(transform.forward, directionToPlayer) < angleVision)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, playerDetectionRange))
            {
                if (hit.collider.gameObject == player)
                {
                    currentState = NPCState.Alert;
                    navMeshAgent.isStopped = false;
                    animator.SetBool("Running", true);
                    animator.SetBool("Attacking", false);
                }
            }
        }
    }

    private void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > stopChaseDistance)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            navMeshAgent.ResetPath();
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currentState = NPCState.Attack;
            animator.SetBool("Running", false);
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > playerDetectionRange)
        {
            currentState = NPCState.Idle;
            navMeshAgent.isStopped = true;
            animator.SetBool("Running", false);
            animator.SetBool("Attacking", false);
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            currentState = NPCState.Alert;
            if (attackSound) attackSound.Play();
            animator.SetBool("Attacking", false);
            animator.SetBool("Running", true);
            return;
        }

        if (canAttack)
        {
            canAttack = false;
            animator.SetBool("Attacking", true);
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(0.5f); // Simula el tiempo de la animación de ataque
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.2f); // Tiempo en el que el golpe es efectivo
        attackCollider.enabled = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 frontRayPoint = transform.position + (transform.forward * playerDetectionRange);
        Vector3 leftRayPoint = Quaternion.Euler(0, angleVision * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -angleVision * 0.5f, 0) * frontRayPoint;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, frontRayPoint);
        Gizmos.DrawLine(transform.position, leftRayPoint);
        Gizmos.DrawLine(transform.position, rightRayPoint);
    }
}
