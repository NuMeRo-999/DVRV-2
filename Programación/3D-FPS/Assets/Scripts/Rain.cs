using UnityEngine;

public class Rain : MonoBehaviour
{
    private float rainSmoothSpeed = .2f;
    public Transform player;
    Vector3 currentPlayerPos;
    private Vector3 velocity = Vector3.zero;


    void Update()
    {
        currentPlayerPos = new Vector3(player.localPosition.x, player.localPosition.y + 15, player.localPosition.z);

        transform.position = Vector3.SmoothDamp(transform.position, currentPlayerPos, ref velocity, rainSmoothSpeed);
    }
}