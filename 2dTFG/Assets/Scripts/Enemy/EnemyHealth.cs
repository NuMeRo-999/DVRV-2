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
        
        // Asegurarse que la capa está entre 0-31
        if (invulnerableLayer.value >= 0 && invulnerableLayer.value <= 31)
        {
            gameObject.layer = invulnerableLayer;
        }
        else
        {
            Debug.LogError("Invalid layer for invulnerableLayer!");
            gameObject.layer = 0; // Capa por defecto
        }
        
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