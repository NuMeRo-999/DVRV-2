using Cainos.CustomizablePixelCharacter;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración")]
    public int maxHealth = 100;
    public int currentHealth;
    public float invulnerabilityTime = 1f;        // Tiempo de invulnerabilidad después de recibir daño
    public float deathRestartDelay = 2f;          // Tiempo de espera antes de reiniciar la escena

    [Header("Referencias")]
    public PixelCharacter character;             // Referencia al script PixelCharacter
    private float invulnerabilityTimer;          // Temporizador de invulnerabilidad
    private float deathTimer = -1f;              // Temporizador para reiniciar la escena

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

        if (character.IsDead)
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            if (deathTimer > 0)
            {
                deathTimer -= Time.deltaTime;
                if (deathTimer <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
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
        GetComponent<PixelCharacterController>().enabled = false;
        deathTimer = deathRestartDelay;
    }

    public void Heal(float percentage)
    {
        // Convertir el porcentaje a un valor entre 0 y 1
        float normalizedPercentage = percentage / 100f;

        // Calcular la cantidad de vida a curar
        int healAmount = Mathf.CeilToInt(maxHealth * normalizedPercentage);

        // Aplicar la curación
        currentHealth += healAmount;

        // Limitar la vida al máximo permitido
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
