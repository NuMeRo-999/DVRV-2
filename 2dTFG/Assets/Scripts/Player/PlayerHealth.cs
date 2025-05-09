using Cainos.CustomizablePixelCharacter;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración")]
    public int maxHealth = 100;
    public int currentHealth;
    public float invulnerabilityTime = 1f;        // Tiempo de invulnerabilidad después de recibir daño

    [Header("Referencias")]
    public PixelCharacter character;             // Referencia al script PixelCharacter
    private float invulnerabilityTimer;          // Temporizador de invulnerabilidad

    private void Start()
    {
        currentHealth = maxHealth;

        if (character == null)
            character = GetComponent<PixelCharacter>();
    }

    private void Update()
    {
        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (invulnerabilityTimer > 0 || character.IsDead) return;

        currentHealth -= damage;
        invulnerabilityTimer = invulnerabilityTime;

        // Animación de daño (front/back según dirección)
        if (hitDirection.x > 0) // Golpe desde la derecha
        {
            character.InjuredFront();
        }
        else // Golpe desde la izquierda
        {
            character.InjuredBack();
        }

        // Muerte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        character.IsDead = true;
        // Aquí puedes añadir más lógica: Game Over, respawn, etc.
    }

    // Método para curar al jugador
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}