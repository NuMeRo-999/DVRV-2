using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public float lookSensitivity = 1f;

    private CharacterController characterController;

    private Vector2 inputDirection;
    private Vector2 lookInput;
    private Vector3 velocity;
    public bool isGrounded;
    public bool isSliding;
    private Vector3 hitNormal;

    private float xRotation = 0f;
    public Platform platform;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckSlope();
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isSliding)
        {
            SlideDown();
        }
        else
        {
            Vector3 move = new Vector3(inputDirection.x, 0, inputDirection.y);
            move = transform.TransformDirection(move);
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        transform.Rotate(0, lookInput.x * lookSensitivity * Time.deltaTime, 0);

        xRotation -= lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void CheckSlope()
    {
        // Considerar solo pendientes pronunciadas para el deslizamiento
        float slopeAngle = Vector3.Angle(Vector3.up, hitNormal);
        isSliding = isGrounded && slopeAngle > characterController.slopeLimit && slopeAngle < 89f; // Ignorar paredes verticales para poder subir escaleras
    }


    private void SlideDown()
    {
        // Dirección del deslizamiento
        Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hitNormal);
        characterController.Move(slideDirection * moveSpeed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

        if (hit.collider.tag == "platform")
        {
            platform = hit.collider.GetComponent<Platform>();
            transform.position = Vector3.MoveTowards(transform.position, platform.platformPositions[platform.GetNextPosition()].position, 0.1f);
            transform.position += platform.speed * Time.deltaTime * Vector3.up;
        }
    }

    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (isGrounded && value.isPressed)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
