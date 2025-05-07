using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parralaxEffect;
    private Transform camera;
    private Vector3 lastCameraPosition;

    void Start()
    {
        camera = Camera.main.transform;
        lastCameraPosition = camera.position;

    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = camera.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parralaxEffect, deltaMovement.y, 0);
        lastCameraPosition = camera.position;
    }
}
