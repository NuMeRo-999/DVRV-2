using System.Drawing;
using TMPro;
using UnityEngine;

public class PointsText : MonoBehaviour
{

    public PointsManager pointsManager;
    public TextMeshProUGUI lovePointsText;
    public TextMeshProUGUI equityPointsText;
    public TextMeshProUGUI timeText;

    public GameObject winPanel;

    public bool winPanelShown = false;

    void Start()
    {
        winPanel.SetActive(false);
    }

    void Update()
    {
        pointsManager = FindAnyObjectByType<PointsManager>();

        if (winPanelShown)
        {
            winPanel.SetActive(true);
        }
    }

    public void showWinPanel()
    {
        lovePointsText.text = "Primera puntuación: <color=#FFFF00>" + pointsManager.lovePoints.ToString() + "</color>";
        equityPointsText.text = "Segunda puntuación: <color=#FFFF00>" + pointsManager.equityPoints.ToString() + "</color>";

        int minutes = Mathf.FloorToInt(pointsManager.timer / 60);
        int seconds = Mathf.FloorToInt(pointsManager.timer % 60);
        timeText.text = "Tiempo: <color=#FFFF00>" + string.Format("{0:00}:{1:00}", minutes, seconds) + "</color>";
        winPanelShown = true;
    }
}
