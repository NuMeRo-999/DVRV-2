using UnityEngine;

public class KeyPad : MonoBehaviour
{
    public int counter = 0;
    public GameObject cube;
    void Start()
    {

    }

    void Update()
    {
        if (counter == 4)
        {
            cube.SetActive(true);
        }
    }

    public void addPoint()
    {
        counter++;
    }
}
