using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 moveDirection;

    void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}

