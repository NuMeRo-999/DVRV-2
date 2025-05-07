using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    float playerHeight = 2f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] Transform head;
    [SerializeField] Transform weapon;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 5f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float acceleration = 10f;
    private bool isSprinting = false;

    [Header("Crouching and Sliding")]
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float slideSpeed = 8f;
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float slideDuration = 1f;
    [SerializeField] CapsuleCollider playerCollider;

    [Header("Drag")]
    float groundDrag = 6f;
    float airDrag = 2f;

    [Header("Ground Detection")]
    [SerializeField] LayerMask groundMask;
    public bool isGrounded;
    float groundDistance = 0.4f;

    [Header("AirDash")]
    public float dashForce = 10f;
    public float dashCounter = 1f;

    [Header("Double Jump")]
    public bool canDoubleJump = true;
    public float jumpCounter = 1f;

    public float gravity = -15f;
    public ConstantForce cf;

    private WallRun wallRun;
    private Vector2 movementInput;
    public Vector2 lookInput;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;
    RaycastHit slopeHit;

    public bool isCrouching = false;
    private bool isSliding = false;
    private float originalHeight;
    private float slideTimer = 0f;

    [Header("Swing Settings")]
    public bool isSwinging;
    public float swingingSpeed = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cf = GetComponent<ConstantForce>();
        wallRun = GetComponent<WallRun>();

        originalHeight = playerHeight;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);

        if (!isGrounded && !wallRun.isWallRunning)
        {
            cf.force = new Vector3(0, gravity, 0);
        }
        else
        {
            cf.force = Vector3.zero;

            if (isGrounded)
            {
                jumpCounter = 1f;
                dashCounter = 1f;
            }
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        ControlDrag();
        ControlSpeed();

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }
    }

    private void FixedUpdate()
    {
        moveDirection = head.forward * movementInput.y + head.right * movementInput.x;

        if (!isGrounded && !onSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && onSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
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

    public void OnJump(InputValue value)
    {
        if (isGrounded && !isCrouching)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (!isGrounded && jumpCounter != 0 && !wallRun.isWallRunning)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCounter--;
        }
    }

    void ControlDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : airDrag;
    }

    void ControlSpeed()
    {
        float targetSpeed = isSliding ? slideSpeed :
                            isCrouching ? crouchSpeed :
                            isSwinging ? swingingSpeed :
                            isSprinting ? runSpeed : walkSpeed;

        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    public void OnCrouch(InputValue value)
    {
        if (value.isPressed)
        {
            StartCrouch();
        }
        else if (!value.isPressed && !Physics.Raycast(transform.position, Vector3.up, 2f))
        {
            StopCrouch();
        }
    }

    void StartCrouch()
    {
        isCrouching = true;
        playerHeight = crouchHeight;
        playerCollider.height = crouchHeight;

        if (isSprinting && isGrounded)
        {
            StartSlide();
        }
    }

    void StopCrouch()
    {
        isCrouching = false;
        playerHeight = originalHeight;
        playerCollider.height = originalHeight;
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
    }

    void StopSlide()
    {
        isSliding = false;
        slideTimer = 0f;
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && !isGrounded && dashCounter != 0)
        {
            Vector3 dashDirection = movementInput != Vector2.zero ? head.forward * movementInput.y + head.right * movementInput.x : head.forward;
            rb.AddForce(dashDirection.normalized * dashForce, ForceMode.Impulse);
            dashCounter--;
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    private bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            return slopeHit.normal != Vector3.up;
        }
        return false;
    }
}
