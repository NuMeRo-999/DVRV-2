using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int health = 100;
    public int maxHealth = 100;
    public GameObject gameOverPanel;
    public GameObject healthSlider;
    public Volume volume;


    void Start()
    {
        gameOverPanel.SetActive(false);
        volume.enabled = false;
        healthSlider.GetComponent<Slider>().value = health;
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthSlider.GetComponent<Slider>().value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.GetComponent<Slider>().value = health;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverPanel.SetActive(true);
        GetComponent<PlayerMovement>().enabled = false;
        volume.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Blood")) {
            Die();
        }
    }
}
