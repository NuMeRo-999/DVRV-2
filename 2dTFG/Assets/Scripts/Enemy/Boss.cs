using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;

public class Boss : MonoBehaviour
{
    [Header("Configuración del Boss")]
    public bool isActive = false;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 25;
    public float attackPushForce = 5f;
    public LayerMask playerLayer;

    private MonsterController monsterController;
    private PixelMonster pixelMonster;
    private Transform player;
    private PlayerHealth playerHealth;
    private float attackTimer = 0f;
    private Rigidbody2D rb;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        pixelMonster = GetComponent<PixelMonster>();
        rb = GetComponent<Rigidbody2D>();
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
        if (!isActive || pixelMonster.IsDead || playerHealth == null || playerHealth.character.IsDead)
        {
            monsterController.inputMove = Vector2.zero;
            return;
        }

        attackTimer -= Time.deltaTime;
        ChaseOrAttackPlayer();
    }

    private void ChaseOrAttackPlayer()
    {
        if (player == null) return;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Ajustar la dirección del boss
        pixelMonster.Facing = directionToPlayer.x > 0 ? -1 : 1;

        if (distanceToPlayer <= attackRange)
        {
            // Ataque
            if (attackTimer <= 0)
            {
                monsterController.Attack(true);
                attackTimer = attackCooldown;
                playerHealth.TakeDamage(attackDamage, directionToPlayer.normalized);

                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.AddForce(directionToPlayer.normalized * attackPushForce, ForceMode2D.Impulse);
                }
            }

            // Detener el movimiento al atacar
            monsterController.inputMove = Vector2.zero;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            // Moverse hacia el jugador para atacar
            Vector2 moveDirection = directionToPlayer.normalized;
            monsterController.inputMove = new Vector2(moveDirection.x, 0);
            monsterController.inputMoveModifier = true;
        }
        else
        {
            // Detenerse si el jugador está fuera del rango de detección
            monsterController.inputMove = Vector2.zero;
        }
    }

    public void ActivateBoss(bool active)
    {
        isActive = active;
    }
}
