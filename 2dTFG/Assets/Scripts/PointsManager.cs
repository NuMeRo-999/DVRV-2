using UnityEngine;

public class PointsManager : MonoBehaviour
{

    public int points = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }

    
}
