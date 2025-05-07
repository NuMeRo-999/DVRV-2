using UnityEngine;

public class Piece : MonoBehaviour
{
    void Start()
    {
        Invoke("CheckWalls", 5);
    }

    public void CheckWalls()
    {
        if (Physics.Raycast(transform.position, transform.right * -1, 6))
            Destroy(transform.GetChild(3).gameObject);
        if (Physics.Raycast(transform.position, transform.right, 6))
            Destroy(transform.GetChild(2).gameObject);
        if (Physics.Raycast(transform.position, transform.forward * -1, 6))
            Destroy(transform.GetChild(1).gameObject);
        if (Physics.Raycast(transform.position, transform.forward, 6))
            Destroy(transform.GetChild(0).gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * -6);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 6);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * -6);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 6);
    }
}
