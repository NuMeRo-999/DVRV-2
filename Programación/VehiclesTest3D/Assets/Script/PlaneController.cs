using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController : MonoBehaviour
{
    public bool isOn;
    public float speed;
    public float maxVelocity = 500;
    private Rigidbody planeRB;
    private PlayerInput PlayerInput;

    public float rotVel1 = 100;
    public float rotVel2 = 100;

    private float m_horizontalInput;
    private float m_verticalInput;

    // Referencia al objeto de la hélice
    public Transform propeller;
    public float propellerRotationSpeedMultiplier = 10f;

    void Start()
    {
        speed = 0;
        isOn = false;
        planeRB = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.actions["Start"].performed += OnsStart;
    }

    private void OnsStart(InputAction.CallbackContext context)
    {
        isOn = true;
        speed = 0;
    }

    private void MovementUpdate()
    {
        if (Altitud() < 5)
        {
            planeRB.useGravity = false;
        }
        else
        {
            planeRB.useGravity = true;
        }

        if (Input.GetKey(KeyCode.K))
        {
            transform.Rotate(Vector3.left * rotVel1 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.L))
        {
            transform.Rotate(Vector3.right * rotVel1 * Time.deltaTime);
        }
    }

    public void onMove(InputValue value)
    {
        if (isOn)
        {
            m_horizontalInput = value.Get<Vector2>().x;
            m_verticalInput = value.Get<Vector2>().y;
            speed = m_verticalInput;
        }
    }

    private void MovementFixedUpdate()
    {
        transform.position += -transform.right * speed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.N))
        {
            transform.Rotate(Vector3.forward * rotVel1 * Time.deltaTime);
        }

        // Movimiento hacia abajo con la tecla M
        if (Input.GetKey(KeyCode.M))
        {
            transform.Rotate(Vector3.back * rotVel1 * Time.deltaTime);
        }
    }

    private void VelocityControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (speed < maxVelocity)
            {
                speed += 1;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (speed < maxVelocity && speed > 0)
            {
                speed -= 1;
            }
        }
    }

    public int Altitud()
    {
        float floorDistance = 0;
        float maxDistance = 10000f;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance))
        {
            floorDistance = hit.distance;
        }
        return (int)floorDistance;
    }

    private void RotatePropeller()
    {
        if (propeller != null)
        {
            // Rotar la hélice según la velocidad del avión
            propeller.Rotate(Vector3.left, speed * propellerRotationSpeedMultiplier * Time.deltaTime);
        }
    }

    void Update()
    {
        if (isOn)
        {
            MovementUpdate();
            VelocityControl();
            RotatePropeller(); // Llamar a la rotación de la hélice
        }
    }

    private void FixedUpdate()
    {
        if (isOn)
        {
            MovementFixedUpdate();
        }
    }
}
