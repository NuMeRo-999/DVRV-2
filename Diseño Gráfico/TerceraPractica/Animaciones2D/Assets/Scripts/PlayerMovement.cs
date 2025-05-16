using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    public GameObject androidController;
    public Button leftButton;
    public Button rightButton;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        #if UNITY_ANDROID
            androidController.SetActive(true);
            leftButton.onClick.AddListener(() => MoveLeft());
            rightButton.onClick.AddListener(() => MoveRight());
        #else
            androidController.SetActive(false);
        #endif
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    void MoveLeft()
    {
        movementInput = Vector2.left;
    }

    void MoveRight()
    {
        movementInput = Vector2.right;
    }

    public void StopMovement()
    {
        movementInput = Vector2.zero;
    }
}
