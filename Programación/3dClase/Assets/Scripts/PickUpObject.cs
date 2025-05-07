using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PickUpObject : MonoBehaviour
{
    public Transform mochila;
    private GameObject pickObject;

    void Start()
    {
        pickObject = null;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.tag == "PickCube" && pickObject == null) {
            pickObject = hit.collider.gameObject;
            pickObject.transform.position = mochila.transform.position;
            hit.transform.parent = mochila;
            hit.rigidbody.isKinematic = true;
        }
    }

    public void OnCrouch(InputValue value)
    {
        if (value.isPressed && pickObject != null)
        {
            pickObject.transform.parent = null;
            pickObject.GetComponent<Rigidbody>().isKinematic = false;
            pickObject = null;
        }
    }
}
