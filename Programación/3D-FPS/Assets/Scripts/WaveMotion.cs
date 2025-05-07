using UnityEngine;

public class WaveMotion : MonoBehaviour
{
    public float amplitudeX = 1f; // Altura de la ola en X
    public float amplitudeY = 1f; // Altura de la ola en Y
    public float frequencyX = 1f; // Frecuencia de la ola en X
    public float frequencyY = 1f; // Frecuencia de la ola en Y
    public float speed = 1f; // Velocidad de propagación

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float time = Time.time * speed;
        float offsetX = Mathf.Sin(time * frequencyX) * amplitudeX;
        float offsetY = Mathf.Cos(time * frequencyY) * amplitudeY;

        transform.position = startPosition + new Vector3(offsetX, offsetY, 0);
    }
}
