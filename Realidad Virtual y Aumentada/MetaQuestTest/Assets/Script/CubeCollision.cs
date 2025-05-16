using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    public GameObject explosion;

    void Start()
    {
        explosion = transform.GetChild(1).gameObject;
        explosion.SetActive(false);
    }

    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube")
        {
            explosion.SetActive(true);

            CubeCollision otherCube = collision.gameObject.GetComponent<CubeCollision>();
            if (otherCube != null)
            {
                otherCube.explosion.SetActive(true);
            }
        }
    }
}
