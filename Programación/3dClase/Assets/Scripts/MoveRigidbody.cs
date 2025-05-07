using UnityEngine;

public class MoveRigidbody : MonoBehaviour
{
    public float speed = 5f;
    private float targetMass;

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null && !rb.isKinematic && hit.moveDirection.y > -0.3f) {
            targetMass = rb.mass;
            Vector3 direction = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.linearVelocity = direction * speed/targetMass;
        }
    }
}
