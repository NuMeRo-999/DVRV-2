using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Vector3 posicionFinal;
    private Vector3 posicionInicio;
    private bool moviendoFin;

    public PlayerController playerController;

    void Start()
    {
        moviendoFin = true;
        posicionInicio = transform.position;

        // posicionFinal = Vector3(); 

    }

    void Update()
    {
        if(playerController.dead) return;
        Vector3 posicionDestino = moviendoFin ? posicionFinal : posicionInicio;
        transform.position = Vector3.MoveTowards(transform.position, posicionDestino, speed * Time.deltaTime);
        if (transform.position == posicionDestino)
        {
            moviendoFin = !moviendoFin;
        }

        // if (moviendoFin)
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, posicionFinal, speed * Time.deltaTime);

        //     if (transform.position == posicionFinal)
        //     {
        //         moviendoFin = false;
        //     }
        // }
        // else
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, posicionInicio, speed * Time.deltaTime);

        //     if (transform.position == posicionInicio)
        //     {
        //         moviendoFin = true;
        //     }
        // }
    }
}
