using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    [Header("Wheels")]
    public WheelCollider frontLeftWheelCollider;
    public GameObject frontLeftWheel;
    [Space(10)]
    public WheelCollider frontRightWheelCollider;
    public GameObject frontRightWheel;
    [Space(10)]
    public WheelCollider backLeftWheelCollider;
    public GameObject backLeftWheel;
    [Space(10)]
    public WheelCollider backRightWheelCollider;
    public GameObject backRightWheel;

    private Rigidbody carRB;

    [Header("Parameters")]
    public float maxSteerAngle = 30;
    public float enginePower = 100;
    public float maxSpeed = 120;
    public int brakeForce = 100;
    public int descelerationForce = 1000;
    public int brake;

    public bool carOn = true;
    public float speed;

    private PlayerInput playerInput;

    public AudioSource engineSound;
    public AudioSource hornSound;
    public AudioSource skidSound; // Nuevo sonido para el derrape

    private bool isSkidding = false; // Para evitar reproducir el sonido repetidamente

    void Start()
    {
        carRB = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        carRB.centerOfMass = Vector3.zero;

        if (playerInput.actions.FindAction("Stop") != null)
        {
            playerInput.actions["Stop"].performed += OnsStop;
            playerInput.actions["Stop"].canceled += OnNoStop;
        }

        ReduceWheelFriction(1.5f, 1.5f);
    }

    void FixedUpdate()
    {
        Acelerate();
        UpdateWheelPoses();
        Steer();
        Brake();
        UpdateEngineSound();
        CheckForSkid(); // Llamada al método para comprobar el derrape
    }

    private void UpdateEngineSound()
    {
        if (engineSound != null)
        {
            float normalizedSpeed = Mathf.Clamp01(carRB.linearVelocity.magnitude / maxSpeed);
            engineSound.pitch = Mathf.Lerp(0.8f, 2.0f, normalizedSpeed);
        }
    }

    private void CheckForSkid()
    {
        WheelHit hit;
        bool skidding = false;

        // Comprobar si alguna rueda está derrapando
        if (frontLeftWheelCollider.GetGroundHit(out hit) && Mathf.Abs(hit.sidewaysSlip) > 0.4f) skidding = true;
        if (frontRightWheelCollider.GetGroundHit(out hit) && Mathf.Abs(hit.sidewaysSlip) > 0.4f) skidding = true;
        if (backLeftWheelCollider.GetGroundHit(out hit) && Mathf.Abs(hit.sidewaysSlip) > 0.4f) skidding = true;
        if (backRightWheelCollider.GetGroundHit(out hit) && Mathf.Abs(hit.sidewaysSlip) > 0.4f) skidding = true;

        if (skidding && !isSkidding)
        {
            skidSound.Play();
            isSkidding = true;
        }
        else if (!skidding && isSkidding)
        {
            skidSound.Stop();
            isSkidding = false;
        }
    }

    public void OnMove(InputValue value)
    {
        if (carOn)
        {
            m_horizontalInput = value.Get<Vector2>().x;
            m_verticalInput = value.Get<Vector2>().y;
        }
    }

    private void OnNoStop(InputAction.CallbackContext context)
    {
        if (carOn)
        {
            brake = descelerationForce;
        }
    }

    private void OnsStop(InputAction.CallbackContext context)
    {
        brake = 0;
        speed = 0;
    }

    public void OnStart(InputValue value)
    {
        if (carOn && carRB.linearVelocity.magnitude < 0.1f) carOn = false;
        else if (!carOn) carOn = true; engineSound.Play();
    }

    private void Acelerate()
    {
        if (carRB.linearVelocity.magnitude * 4 > maxSpeed)
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = m_verticalInput * enginePower;
            frontRightWheelCollider.motorTorque = m_verticalInput * enginePower;
        }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheel.transform);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheel.transform);
        UpdateWheelPose(backLeftWheelCollider, backLeftWheel.transform);
        UpdateWheelPose(backRightWheelCollider, backRightWheel.transform);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 wheelPos = wheelTransform.position;
        Quaternion _quat = transform.rotation;
        wheelCollider.GetWorldPose(out wheelPos, out _quat);
        wheelTransform.position = wheelPos;
        wheelTransform.rotation = _quat;
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;

        frontLeftWheelCollider.steerAngle = Mathf.Lerp(frontLeftWheelCollider.steerAngle, m_steeringAngle, Time.deltaTime * 5);
        frontRightWheelCollider.steerAngle = Mathf.Lerp(frontRightWheelCollider.steerAngle, m_steeringAngle, Time.deltaTime * 5);
    }

    private void Brake()
    {
        if (m_verticalInput == 0)
        {
            frontLeftWheelCollider.brakeTorque = brake;
            frontRightWheelCollider.brakeTorque = brake;
            backLeftWheelCollider.brakeTorque = brake;
            backRightWheelCollider.brakeTorque = brake;
        }
        else
        {
            frontLeftWheelCollider.brakeTorque = 0;
            frontRightWheelCollider.brakeTorque = 0;
            backLeftWheelCollider.brakeTorque = 0;
            backRightWheelCollider.brakeTorque = 0;
        }
    }

    private void ReduceWheelFriction(float frictionFront, float frictionBack)
    {
        WheelCollider[] wheelColliders = { frontLeftWheelCollider, frontRightWheelCollider, backLeftWheelCollider, backRightWheelCollider };

        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            WheelFrictionCurve frictionCurve = wheelCollider.forwardFriction;
            frictionCurve.extremumSlip = 1f;
            frictionCurve.extremumValue = frictionFront;
            frictionCurve.asymptoteSlip = 2f;
            frictionCurve.asymptoteValue = frictionFront * 0.5f;
            frictionCurve.stiffness = frictionFront;
            wheelCollider.forwardFriction = frictionCurve;

            frictionCurve = wheelCollider.sidewaysFriction;
            frictionCurve.extremumSlip = 1f;
            frictionCurve.extremumValue = frictionBack;
            frictionCurve.asymptoteSlip = 2f;
            frictionCurve.asymptoteValue = frictionBack * 0.5f;
            frictionCurve.stiffness = frictionBack;
            wheelCollider.sidewaysFriction = frictionCurve;
        }
    }

    public void OnInteract(InputValue value)
    {
        if (carOn)
        {
            hornSound.Play();
        }
    }
}
