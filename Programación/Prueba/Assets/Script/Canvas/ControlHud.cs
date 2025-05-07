using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlHud : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerUpText;
    public TextMeshProUGUI lifeText;
    public int vida = 3;
    public int puntos = 0;

    void Start()
    {
        lifeText.text = "Vidas: " + vida;
        scoreText.text = "Puntos: " + puntos;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Puntos: " + score;
    }

    public void UpdatePowerUp(int powerUp)
    {
        powerUpText.text = "PowerUp: " + powerUp;
    }

    public void UpdateLife(int life)
    {
        lifeText.text = "Vidas: " + life;
    }

    void Update()
    {
        
    }
}
