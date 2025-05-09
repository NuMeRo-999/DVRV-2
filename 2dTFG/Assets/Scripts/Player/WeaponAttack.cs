using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [Header("Configuración")]
    public int damage = 10;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;
    public Vector2 attackOffset = new Vector2(0.5f, 0);
    public float attackRadius = 0.8f;

    [Header("Referencias")]
    public Animator animator;
    private float lastAttackTime;
    private bool isAttacking = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
        Invoke("DoDamage", 0.2f); // Pequeño delay para sincronizar con animación
    }

    private void DoDamage()
    {
        if (!isAttacking) return;

        Vector2 attackPos = (Vector2)transform.position + attackOffset * transform.localScale.x;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                Vector2 hitDirection = (enemy.transform.position - transform.position).normalized;
                enemyHealth.TakeDamage(damage, hitDirection);
            }
        }
        
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null) return;
        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + attackOffset * transform.localScale.x;
        Gizmos.DrawWireSphere(attackPos, attackRadius);
    }
}   