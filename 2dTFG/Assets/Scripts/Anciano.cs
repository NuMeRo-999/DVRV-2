using UnityEngine;
using Cainos.CustomizablePixelCharacter;

public class Anciano : MonoBehaviour
{
    public Transform player;
    public bool followingPlayer = false;

    public float moveSpeed = 2f;
    
    private PixelCharacterController controller;
    private PixelCharacter character;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<PixelCharacterController>();
        character = GetComponent<PixelCharacter>();
    }

    void Update()
    {
        if (followingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            // Detener el movimiento si no está siguiendo al jugador
            controller.inputMove = Vector2.zero;
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        // Calcular la dirección hacia el jugador
        Vector3 direction = player.position - transform.position;
        
        // Solo nos interesa la dirección horizontal para el movimiento
        float horizontalInput = Mathf.Clamp(direction.x, -1f, 1f);
        
        // Configurar el input de movimiento del controlador
        controller.inputMove = new Vector2(horizontalInput, 0);

        // Mover al personaje hacia el jugador
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        
        // Determinar la dirección de mirada
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            character.Facing = horizontalInput > 0 ?
                PixelCharacter.FacingType.Right :
                PixelCharacter.FacingType.Left;
        }
    }

  public void OnTriggerEnter2D(Collider2D collision)
  {
      print(collision.tag);
  }
}
