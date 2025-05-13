using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;

public class PointsManager : MonoBehaviour
{
    public int points = 0;
    public float timer = 0f;
    public bool isTimerRunning = true;
    
    [Header("UI Elements")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timerText;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        points = DialogueLua.GetVariable("Points").AsInt;
        timer = DialogueLua.GetVariable("Timer").AsFloat;
    }

    void Update()
    {
        // Actualizar temporizador si está activo
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            DialogueLua.SetVariable("Timer", timer);
        }

        // Sincronizar puntos si cambian externamente
        int currentPoints = DialogueLua.GetVariable("Points").AsInt;
        if (points != currentPoints)
        {
            points = currentPoints;
            Debug.Log("Points updated from Lua: " + points);
        }

        // Actualizar la UI
        UpdateUI();
    }

    public void AddPoints(float pointsToAdd, float secondsToAdd)
    {
        int roundedPoints = Mathf.RoundToInt(pointsToAdd);
        points += roundedPoints;
        DialogueLua.SetVariable("Points", points);

        // Añadir segundos al temporizador
        timer += secondsToAdd;
        DialogueLua.SetVariable("Timer", timer);
    }

    public void ResetTimer()
    {
        timer = 0f;
        DialogueLua.SetVariable("Timer", timer);
        Debug.Log("Timer reset to 0");
    }

    private void UpdateUI()
    {
        if (pointsText != null)
            pointsText.text = "Points: " + points;

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
