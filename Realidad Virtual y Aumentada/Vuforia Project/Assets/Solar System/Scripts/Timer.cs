using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{
    public float countdownTime;
    public TextMeshProUGUI timerText;
    [SerializeField] public float currentTime;

    void Start()
    {
        currentTime = countdownTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(currentTime);
            timerText.text = string.Format("{0:00}", seconds);
        }
        else
        {
            timerText.text = "¡Tiempo terminado!";
        }
    }
}
