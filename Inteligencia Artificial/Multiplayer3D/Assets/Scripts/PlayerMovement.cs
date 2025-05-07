using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 _velocity;
    private bool _jumpPressed;

    private CharacterController _controller;

    public float PlayerSpeed = 2f;

    public float JumpForce = 5f;
    public float GravityValue = -9.81f;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform camTransform;

    private Vector2 cameraInput;
    public float sensitivity = 10f;
    private float verticalRotation;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        // cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        // FixedUpdateNetwork is only executed on the StateAuthority

        if (_controller.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        _velocity.y += GravityValue * Runner.DeltaTime;
        if (_jumpPressed && _controller.isGrounded)
        {
            _velocity.y += JumpForce;
        }
        _controller.Move(move + _velocity * Runner.DeltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

    
        Vector2 look = cameraInput * sensitivity;
        verticalRotation -= look.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * look.x);

        _jumpPressed = false;
    }

    public void onLook(InputValue value)
    {
        if(!HasStateAuthority)
        {
            cameraInput = value.Get<Vector2>();
        }
    }

    public Camera GetCamera()
    {
        return playerCamera;
    }
    
    public void SetCamera(Camera camera)
    {
        playerCamera = camera;
        playerCamera.transform.position = camTransform.position;
        playerCamera.transform.rotation = camTransform.rotation;
        playerCamera.transform.SetParent(transform);
    }
}