using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform Target;
    public float MouseSensitivity = 10f;

    private float verticalRotation;
    private float horizontalRotation;

    void Start()
    {
        transform.position = Target.position;
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = Target.position;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    // public void SetTarget()
    // {
    //     Target = GameObject.FindGameObjectWithTag("Player").transform;
    //     transform.position = Target.position;
    //     transform.rotation = Target.rotation;
    //     transform.SetParent(Target);
    // }
}