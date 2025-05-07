using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float velocidad = 3;
    public Vector3 posFinal;
    private Vector3 posInicial;
    private bool moviendoFin;
    void Start()
    {
        posInicial = transform.position;
        moviendoFin = true;
    }


    void Update()
    {
        Vector3 posDestino = moviendoFin ? posFinal : posInicial;
        transform.position = Vector3.MoveTowards(transform.position, posDestino, velocidad * Time.deltaTime);

        if (transform.position == posDestino)
        {
            moviendoFin = !moviendoFin;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 collisionNormal = other.contacts[0].normal;
            if (collisionNormal.y < -0.5f)
            {
                other.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}