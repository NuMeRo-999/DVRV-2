using UnityEngine;

public class JumpIndicator : MonoBehaviour
{

    public GameObject jumpIndicator;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jumpIndicator.SetActive(true);
        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        jumpIndicator.SetActive(false);
    }
}
