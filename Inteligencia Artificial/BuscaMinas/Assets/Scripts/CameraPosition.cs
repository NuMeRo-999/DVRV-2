using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public GameObject gridMapGenerator;

    void Start()
    {
        int width = ControlDatosJuego.Instance.Width;
        int height = ControlDatosJuego.Instance.Height;
        this.transform.position = new Vector3(((float)width * 0.16f / 2) - 0.08f, ((float)height * 0.16f / 2) - 0.08f, -10);
        Camera.main.orthographicSize = (Mathf.Max(width, height) * 0.16f / 2) + 0.2f;
    }
}
