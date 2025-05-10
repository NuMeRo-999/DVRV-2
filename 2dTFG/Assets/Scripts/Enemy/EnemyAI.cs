using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.5f;
    public float runSpeed = 5.0f;
    public float waitTimeAtPoint = 1f;
    public float reachThreshold = 2f;

    [Header("Player Detection")]
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public int attackDamage = 10;
    public float attackCooldown = 1f;
    public float attackPushForce = 2f;

    [Header("Debug")]
    public EnemyState currentState = EnemyState.Patrolling;
    public bool isGrounded;

    private MonsterController monsterController;
    private PixelMonster pixelMonster;
    private Transform player;
    private PlayerHealth playerHealth;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private float attackTimer = 0f;
    private bool isWaiting = false;
    private Rigidbody2D rb;
    private BoxCollider2D enemyCollider;

    public enum EnemyState { Patrolling, Chasing, Attacking }

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        pixelMonster = GetComponent<PixelMonster>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (pixelMonster.IsDead || playerHealth == null || playerHealth.character.IsDead)
        {
            monsterController.inputMove = Vector2.zero;
            currentState = EnemyState.Patrolling; // Regresa al estado de patrulla si el jugador está muerto
            return;
        }

        CheckGrounded();

        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.Patrolling:
                PatrolBehavior();
                CheckForPlayer();
                break;

            case EnemyState.Chasing:
                ChaseBehavior();
                CheckForPlayer();
                break;

            case EnemyState.Attacking:
                AttackBehavior();
                break;
        }
    }

    private void CheckGrounded()
    {
        Vector2 boxSize = new Vector2(enemyCollider.bounds.size.x * 0.9f, 0.1f);
        Vector2 boxCenter = new Vector2(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCenter,
            boxSize,
            0f,
            Vector2.down,
            0.05f,
            groundLayer);

        isGrounded = hit.collider != null;
        pixelMonster.IsGrounded = isGrounded;
    }

    private void PatrolBehavior()
    {
        if (patrolPoints.Length == 0 || !isGrounded)
        {
            monsterController.inputMove = Vector2.zero;
            return;
        }

        if (isWaiting)
        {
            monsterController.inputMove = Vector2.zero;
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoint)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
            return;
        }

        Vector2 direction = patrolPoints[currentPatrolIndex].position - transform.position;
        float distance = direction.magnitude;

        if (distance <= reachThreshold)
        {
            isWaiting = true;
            monsterController.inputMove = Vector2.zero;
            return;
        }

        direction.Normalize();
        monsterController.inputMove = new Vector2(direction.x, 0);
        monsterController.inputMoveModifier = false;
        pixelMonster.Facing = direction.x > 0 ? 1 : -1;
    }

    private void ChaseBehavior()
    {
        if (player == null || playerHealth.character.IsDead)
        {
            currentState = EnemyState.Patrolling;
            return;
        }

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= attackRange)
        {
            currentState = EnemyState.Attacking;
            monsterController.inputMove = Vector2.zero;
            attackTimer = attackCooldown;
            return;
        }

        direction.Normalize();
        monsterController.inputMove = new Vector2(direction.x, 0);
        monsterController.inputMoveModifier = true;
        pixelMonster.Facing = direction.x > 0 ? 1 : -1;
    }

    private void AttackBehavior()
    {
        if (player == null || playerHealth.character.IsDead)
        {
            currentState = EnemyState.Patrolling;
            return;
        }

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > attackRange * 1.2f)
        {
            currentState = EnemyState.Chasing;
            return;
        }

        monsterController.inputMove = Vector2.zero;
        pixelMonster.Facing = direction.x > 0 ? 1 : -1;

        if (attackTimer <= 0)
        {
            monsterController.Attack(true);
            attackTimer = attackCooldown;

            if (distance <= attackRange)
            {
                playerHealth.TakeDamage(attackDamage, direction.normalized);

                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.AddForce(direction.normalized * attackPushForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void CheckForPlayer()
    {
        if (player == null || playerHealth.character.IsDead) return;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
            return;
        }

        if (distanceToPlayer <= detectionRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                directionToPlayer.normalized,
                detectionRange,
                playerLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                currentState = EnemyState.Chasing;
            }
            else if (currentState == EnemyState.Chasing)
            {
                currentState = EnemyState.Patrolling;
            }
        }
        else if (currentState == EnemyState.Chasing)
        {
            currentState = EnemyState.Patrolling;
        }
    }
}
