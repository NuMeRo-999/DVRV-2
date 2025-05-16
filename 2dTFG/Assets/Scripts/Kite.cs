using UnityEngine;

public class Kite : MonoBehaviour
{
    private PointsManager pointsManager;
    void Start()
    {
        pointsManager = FindAnyObjectByType<PointsManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        print(other.tag);
        if (other.tag == "Player")
        {
            pointsManager.AddPoints(3, 0);
            Destroy(gameObject);
        }
    }
}
