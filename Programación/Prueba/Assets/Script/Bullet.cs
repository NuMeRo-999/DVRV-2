using Cainos.LucidEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 5;
    [SerializeField] private float lifeTime = 10;
    [SerializeField] private int damage = 1;
    public ControlHud ControlHud;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            ControlHud.UpdateScore(ControlHud.puntos + 10);
        }

        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
