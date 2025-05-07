using UnityEngine;
using UnityEngine.UI;

public class AirplaneController : MonoBehaviour
{
    public float maxSpeed = 60f; // Velocidad máxima
    public float acceleration = 40f; // Incremento de velocidad por segundo
    public float deceleration = 5f; // Reducción de velocidad por segundo
    public float rotationSpeed = 500f; // Velocidad de rotación
    public float rotationSmoothing = 5f; // Suavizado de rotación

    private float currentSpeed = 0f; // Velocidad actual del avión
    private Quaternion targetRotation; // Rotación deseada
    private Animator animator; // Referencia al componente Animator

    // Botones
    public Button btnAccelerate;
    public Button btnBrake;
    public Button btnUp;
    public Button btnDown;
    public Button btnLeft;
    public Button btnRight;

    void Start()
    {
        // Asignar eventos a los botones
        btnAccelerate.onClick.AddListener(() => ModifySpeed(acceleration));
        btnBrake.onClick.AddListener(() => ModifySpeed(-deceleration));
        btnUp.onClick.AddListener(() => AddRotation(Vector3.left)); // Subir (rotar hacia atrás)
        btnDown.onClick.AddListener(() => AddRotation(Vector3.right)); // Bajar (rotar hacia adelante)
        btnLeft.onClick.AddListener(() => AddRotation(Vector3.down)); // Girar a la izquierda
        btnRight.onClick.AddListener(() => AddRotation(Vector3.up)); // Girar a la derecha

        // Obtener el componente Animator
        animator = GetComponent<Animator>();

        // Inicializar la rotación deseada como la actual
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Mover el avión hacia adelante según la velocidad actual
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Interpolación hacia la rotación deseada
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);

        // Activar o desactivar la animación según la velocidad
        if (animator != null)
        {
            animator.SetBool("isMoving", currentSpeed > 0);
        }
    }

    void ModifySpeed(float delta)
    {
        // Incrementar o reducir la velocidad
        currentSpeed += delta * Time.deltaTime;

        // Limitar la velocidad dentro del rango permitido
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    void AddRotation(Vector3 direction)
    {
        // Calcular la nueva rotación objetivo
        targetRotation *= Quaternion.Euler(direction * rotationSpeed * Time.deltaTime);
    }
}
