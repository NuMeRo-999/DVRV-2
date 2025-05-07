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
    }

    void FixedUpdate()
    {
        Acelerate();
        UpdateWheelPoses();
        Steer();
        Brake();
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
        carOn = false;
    }

    public void OnStart(InputValue value)
    {
        if (carOn && carRB.linearVelocity.magnitude < 0.1f) carOn = false;
        else if (!carOn) carOn = true;
    }

    private void Acelerate()
    {
        if(carRB.linearVelocity.magnitude * 4 > maxSpeed)
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

        frontLeftWheelCollider.steerAngle = m_steeringAngle;
        frontRightWheelCollider.steerAngle = m_steeringAngle;
    }

    private void Brake()
    {
        if(m_verticalInput == 0)
        {
            frontLeftWheelCollider.brakeTorque = brake;
            frontRightWheelCollider.brakeTorque = brake;
            backLeftWheelCollider.brakeTorque = brake;
            backRightWheelCollider.brakeTorque = brake;
        }
        else
        {
            frontLeftWheelCollider.brakeTorque = 0;
        }
    }
}
