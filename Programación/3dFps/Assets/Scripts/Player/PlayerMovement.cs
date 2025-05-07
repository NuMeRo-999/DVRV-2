using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    private Rigidbody rb;

    [Header("Camera and Arms Settings")]
    public Transform playerCamera;
    public Transform playerArms;
    private float cameraPitch = 0f;

    private Vector2 movementInput;
    private Vector2 lookInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleLook();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        moveDirection.Normalize();

        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    private void HandleLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        cameraPitch -= lookInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        if (playerArms != null)
        {
            playerArms.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

}
