using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class PlayerMouse : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;
    private Vector3 mouseInput;
    private float mouseX;
    private float mouseY;
    private bool mouseDown = false;
    private bool mouseUp = false;
    
    void Start()
    {
    }

    public void OnLook(InputValue value)
    {
        mouseInput = value.Get<Vector2>();
    }

    public void SetWeapon(Transform weapon)
    {
        transform.SetParent(weapon);
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {

    }
}
