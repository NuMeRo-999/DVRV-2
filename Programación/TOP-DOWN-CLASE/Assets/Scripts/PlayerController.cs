using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 smoothMovementInput;
    private Vector2 movementInputSmoothVelocity;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        smoothMovementInput = Vector2.SmoothDamp(
            smoothMovementInput,
            movementInput,
            ref movementInputSmoothVelocity,
            0.1f);

        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
        animator.SetFloat("Speed", movementInput.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.velocity = smoothMovementInput * speed;
    }
}
