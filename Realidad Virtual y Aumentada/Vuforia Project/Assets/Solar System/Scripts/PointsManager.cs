using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Vuforia;
using System.IO;

public class PointsManager : MonoBehaviour
{
    public int points = 0;
    public List<GameObject> planets;
    public TextMeshProUGUI Pointstext;
    public TextMeshProUGUI PlanetText;

    public float countdownTime;
    public TextMeshProUGUI timerText;
    [SerializeField] public float currentTime;

    private int lastPlanetIndex = -1;

    void Start()
    {
        currentTime = countdownTime;

        GetRandomPlanet();
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
            timerText.text = "�Tiempo terminado!";
        }

        if (currentTime <= 0)
        {
            currentTime = countdownTime;
            GetRandomPlanet();
            AddPoints(-5);
        }

    }

    public void CheckAnswer(string planetName)
    {
        if (PlanetText.text == planetName)
        {
            AddPoints(10);
            GetRandomPlanet();
            currentTime = countdownTime;
        }
        else
        {
            timerText.text = "�Incorrecto!";
            GetRandomPlanet();
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        Pointstext.text = "Puntos: " + points;
    }

    public void GetRandomPlanet()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, planets.Count);
        } while (randomIndex == lastPlanetIndex);

        lastPlanetIndex = randomIndex;

        GameObject selectedPlanet = planets[randomIndex];
        PlanetText.text = selectedPlanet.name;
    }

    
  
    
}
