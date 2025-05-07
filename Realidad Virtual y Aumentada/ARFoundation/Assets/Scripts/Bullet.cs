using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem FoodParticleSystem;
    public ParticleSystem BombParticleSystem;

    [SerializeField] private FoodNinja foodNinja;
    

    void Start()
    {
        
    }
    void Update()
    {
        transform.Rotate(0, 0, 360 * Time.deltaTime);
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        ParticleSystem particle = Instantiate(FoodParticleSystem, transform.position, Quaternion.identity);
        var main = particle.main;
        var colorOverLifetime = particle.colorOverLifetime;
        colorOverLifetime.enabled = true;

        Gradient gradient = new Gradient();
        if (collision.gameObject.name == "Pimiento(Clone)")
        {
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        }
        else if (collision.gameObject.name == "Tomato(Clone)")
        {
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        }
        else if (collision.gameObject.name == "Salmon(Clone)")
        {
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color(1.0f, 0.5f, 0.0f), 0.0f), new GradientColorKey(new Color(1.0f, 0.5f, 0.0f), 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        }
        else if (collision.gameObject.name == "Pizza(Clone)")
        {
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        }
        else if (collision.gameObject.name == "Salami(Clone)")
        {
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color(0.6f, 0.2f, 0.2f), 0.0f), new GradientColorKey(new Color(0.6f, 0.2f, 0.2f), 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        }
        else if (collision.gameObject.name == "Melon(Clone)")
        {
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        }

        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        Debug.Log("Nombre: " + collision.gameObject.name);
        if (collision.gameObject.name == "Bomb(Clone)")
        {
            Debug.Log("colision Bomb");
            foodNinja.takeDamage();
            BombParticleSystem.Play();
        }
        else
        {
            FoodParticleSystem.Play();
            foodNinja.addPoints(10);
        }

        Destroy(collision.gameObject);
        Destroy(gameObject);
        Destroy(particle, 3f);
    }
}
