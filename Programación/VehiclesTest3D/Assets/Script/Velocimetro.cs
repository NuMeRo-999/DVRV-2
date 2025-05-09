using UnityEngine;
using UnityEngine.UI;

public class Velocimetro : MonoBehaviour
{

    public Image aguja;
    private Rigidbody carRB;
    public int ajusteAguja = 10;
    public float speed;

    void Start()
    {
        carRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        speed = carRB.linearVelocity.magnitude;
        float smoothSpeed = Mathf.Lerp(aguja.transform.eulerAngles.z, -speed + ajusteAguja, Time.deltaTime * 5);
        aguja.transform.eulerAngles = new Vector3(0, 0, smoothSpeed);
    }
}
