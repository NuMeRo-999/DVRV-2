using UnityEngine;

public class PinCounter : MonoBehaviour
{
    public int fallenPins = 0;
    public TMPro.TextMeshProUGUI text;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pin"))
        {
            fallenPins++;
            other.tag = "FallenPin";
            text.text = fallenPins.ToString();
            gameManager.UpdateScore(1);
        }
    }

    public void ResetCounter()
    {
        fallenPins = 0;
        text.text = "0";
    }
}
