using Cainos.PixelArtMonster_Dungeon;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public string damageTag = "Player";

    [Header("References")]
    public MonsterController monsterController;
    public PixelMonster pixelMonster;
    public LayerMask invulnerableLayer;

    private void Start()
    {
        currentHealth = maxHealth;

        if (pixelMonster == null)
            pixelMonster = GetComponent<PixelMonster>();

        if (monsterController == null)
            monsterController = GetComponent<MonsterController>();
    }

    private void Update()
    {
        if (pixelMonster.IsDead || monsterController.IsDead)
        {
            GetComponent<Rigidbody2D>().linearVelocityX = 0;
            GetComponent<Rigidbody2D>().linearVelocityY = 0;
        }
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (monsterController.IsDead) return;

        currentHealth -= damage;

        if (hitDirection.x > 0)
            pixelMonster.InjuredFront();
        else
            pixelMonster.InjuredBack();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        monsterController.IsDead = true;
        pixelMonster.IsDead = true;
        
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<BoxCollider2D>().enabled = false;
        
        monsterController.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(damageTag))
        {
            Vector2 hitDirection = (transform.position - collision.transform.position).normalized;
            TakeDamage(10, hitDirection);
        }
    }
}