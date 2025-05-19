using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;

public class PointsManager : MonoBehaviour
{
    public int lovePoints = 0;
    public int equityPoints = 0;
    public float timer = 0f;
    public bool isTimerRunning = true;
    
    [Header("UI Elements")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI playerHealthText;

    public Anciano anciano;
    public PlayerHealth playerHealth;
    public Boss boss;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        anciano = FindAnyObjectByType<Anciano>();
        lovePoints = DialogueLua.GetVariable("lovePoints").AsInt;
        equityPoints = DialogueLua.GetVariable("equityPoints").AsInt;
        timer = DialogueLua.GetVariable("Timer").AsFloat;
    }

    void Update()
    {

        playerHealth = FindAnyObjectByType<PlayerHealth>();
        boss = FindAnyObjectByType<Boss>();
        
        // Actualizar temporizador si está activo
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            DialogueLua.SetVariable("Timer", timer);
        }

        // Sincronizar puntos si cambian externamente
        int currentLovePoints = DialogueLua.GetVariable("lovePoints").AsInt;
        int currentEquityPoints = DialogueLua.GetVariable("equityPoints").AsInt;

        if (currentLovePoints != lovePoints || currentEquityPoints != equityPoints)
        {
            lovePoints = currentLovePoints;
            equityPoints = currentEquityPoints;
            DialogueLua.SetVariable("lovePoints", lovePoints);
            DialogueLua.SetVariable("equityPoints", equityPoints);
            Debug.Log("Points updated from Lua: " + lovePoints + ", " + equityPoints);
        }

        // Actualizar la UI
        UpdateUI();
    }

    public void AddPoints(float lovePointsToAdd, float equityPointsToAdd, float secondsToAdd)
    {
        int roundedLovePoints = Mathf.RoundToInt(lovePointsToAdd);
        int roundedEquityPoints = Mathf.RoundToInt(equityPointsToAdd);

        lovePoints += roundedLovePoints;
        equityPoints += roundedEquityPoints;

        DialogueLua.SetVariable("lovePoints", lovePoints);
        DialogueLua.SetVariable("equityPoints", equityPoints);

        // Añadir segundos al temporizador
        timer += secondsToAdd;
        DialogueLua.SetVariable("Timer", timer);
    }

    public void Heal(float percentage)
    {
        playerHealth.Heal(percentage);
    }

    public void FollowPlayer()
    {
        anciano.followingPlayer = true;
    }

    public void ActivateBoss()
    {
        boss.isActive = true;
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
            pointsText.text = "Love: " + lovePoints + " | Equity: " + equityPoints;

        if (playerHealthText != null)
            playerHealthText.text = playerHealth.currentHealth.ToString();

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
