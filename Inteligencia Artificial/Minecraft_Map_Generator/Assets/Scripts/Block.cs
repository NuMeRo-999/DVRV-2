using UnityEngine;

public class Block : MonoBehaviour
{
    void Start()
    {

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        bool allDirectionsHit = true;

        foreach (Vector3 direction in directions)
        {
            if (!Physics.Raycast(transform.position, direction))
            {
            allDirectionsHit = false;
            break;
            }
        }

        if (allDirectionsHit)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }

    }

    void Update()
    {
        
    }
}
