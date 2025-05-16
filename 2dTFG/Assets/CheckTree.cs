using UnityEngine;

public class CheckTree : MonoBehaviour
{
    public Anciano anciano;   

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.tag);
        if (other.CompareTag("Tree"))
        {
            anciano.followingPlayer = false;
        }
    }
}
