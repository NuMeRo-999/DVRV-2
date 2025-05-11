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
    // public Animator animator;
    private float lastAttackTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {

            StartAttack();
        }
    }

    private void StartAttack()
    {
        lastAttackTime = Time.time;
        DoDamage();
        // animator.SetTrigger("Attack");
    }

    // Llama a este método desde el evento de la animación en el momento exacto del impacto
    public void DoDamage()
    {
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
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null) return;
        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + attackOffset * transform.localScale.x;
        Gizmos.DrawWireSphere(attackPos, attackRadius);
    }
}
