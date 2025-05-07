using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    public Rigidbody rb;
    public Transform[] platformPositions;
    private int nextPosition;

    void Start()
    {
        nextPosition = 0;
    }

    void Update()
    {
        MovePlatform();   
    }

    private void MovePlatform()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, platformPositions[GetNextPosition()].position, speed * Time.deltaTime));
    }

    public int GetNextPosition()
    {
        if (rb.position == platformPositions[nextPosition].position)
        {
            if (++nextPosition == platformPositions.Length)
            {
                nextPosition = 0;
            }
        }
            return nextPosition;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
    }
}
