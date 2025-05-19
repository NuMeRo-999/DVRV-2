using UnityEngine;
using Cainos.CustomizablePixelCharacter;

public class Anciano : MonoBehaviour
{
    public Transform player;
    public bool followingPlayer = false;

    public float moveSpeed = 2f;
    public float stopDistance = 1.0f;
    private bool isOnTree = false;

    private PixelCharacterController controller;
    private PixelCharacter character;

    private Vector3 lastPlayerPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<PixelCharacterController>();
        character = GetComponent<PixelCharacter>();
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        PixelCharacter.ExpressionType expression = (followingPlayer || isOnTree) ? PixelCharacter.ExpressionType.Happy : PixelCharacter.ExpressionType.Sick;
        character.Expression = expression;

        if (followingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            controller.inputMove = Vector2.zero;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        // Actualizar la última posición conocida del jugador
        lastPlayerPosition = player.position;
    }

    void FollowPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // Asegurar que siempre esté detrás del jugador
        float playerDirection = Mathf.Sign(player.position.x - lastPlayerPosition.x);
        float targetX = player.position.x - playerDirection * stopDistance;

        // Solo nos interesa la dirección horizontal para el movimiento
        float horizontalInput = Mathf.Clamp(direction.x, -1f, 1f);
        controller.inputMove = new Vector2(horizontalInput, 0);

        // Mover al anciano detrás del jugador
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Ajustar la dirección de la cara del anciano
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            character.Facing = horizontalInput > 0 ? PixelCharacter.FacingType.Right : PixelCharacter.FacingType.Left;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tree"))
        {
            followingPlayer = false;
            isOnTree = true;
        }
    }
}
