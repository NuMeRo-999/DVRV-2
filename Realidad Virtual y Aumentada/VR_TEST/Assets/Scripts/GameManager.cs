using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public int totalRounds = 10;
    public int currentRound = 1;
    public int score = 0;

    public GameObject pinPrefab;
    public GameObject ballsPrefab;
    public PinCounter pinCounter;

    private Vector3[] ballsStartPosition;
    private Vector3[] originalPinPositions;
    private Quaternion[] originalPinRotations;

    public GameObject[] pins;
    public GameObject[] balls;

    public float timer = 5;
    [SerializeField] private GameObject pinsBarrier;

    private bool roundInProgress = true;
    private int attemptsInCurrentRound = 0;
    private bool[] pinsStillStanding;

    void Awake()
    {
        InitializePins();
        InitializeBalls();

        foreach (GameObject ball in balls)
        {
            ball.transform.position = ballsStartPosition[System.Array.IndexOf(balls, ball)];
        }

        pinsStillStanding = new bool[pins.Length];
        for (int i = 0; i < pins.Length; i++) pinsStillStanding[i] = true;
    }

    void Update()
    {
        if (roundInProgress && AllPinsDown())
        {
            roundInProgress = false;
            StartCoroutine(CompleteRound(true)); // pleno
        }
    }

    private void InitializePins()
    {
        var pinTransforms = pinPrefab.GetComponentsInChildren<Transform>()
            .Where(t => t != pinPrefab.transform)
            .ToArray();

        pins = pinTransforms
            .Where(t => t.CompareTag("Pin"))
            .Select(t => t.gameObject)
            .ToArray();

        originalPinPositions = pins.Select(p => p.transform.position).ToArray();
        originalPinRotations = pins.Select(p => p.transform.rotation).ToArray();
    }

    private void InitializeBalls()
    {
        balls = ballsPrefab.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag("Ball"))
            .Select(t => t.gameObject)
            .ToArray();

        ballsStartPosition = balls.Select(b => b.transform.position).ToArray();
    }

    private bool AllPinsDown()
    {
        return pins.All(p => !p.activeSelf);
    }

    private IEnumerator CompleteRound(bool allPinsFallen)
    {
        yield return new WaitForSeconds(2f);

        int fallenThisRound = pinCounter.fallenPins;
        UpdateScore(fallenThisRound);
        pinCounter.fallenPins = 0;

        attemptsInCurrentRound++;

        bool fullReset = false;

        if (allPinsFallen)
        {
            fullReset = true;
            attemptsInCurrentRound = 0;
        }
        else if (attemptsInCurrentRound >= 2)
        {
            fullReset = true;
            attemptsInCurrentRound = 0;
        }
        else
        {
            fullReset = false;
        }

        SaveStandingPins();

        if (fullReset)
            currentRound++;

        if (currentRound > totalRounds)
        {
            Debug.Log("Fin del juego. Puntuación: " + score);
            yield break;
        }

        ResetRound(fullReset);
    }

    private void SaveStandingPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pinsStillStanding[i] = pins[i].activeSelf;
        }
    }

    private void ResetRound(bool fullReset)
    {
        pinsBarrier.GetComponent<Animator>().SetTrigger("OpenClose");

        // Reset balls
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].transform.position = ballsStartPosition[i];

            Rigidbody rb = balls[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        StartCoroutine(ResetPinsAfterDelay(1.5f, fullReset));
    }

    private IEnumerator ResetPinsAfterDelay(float delay, bool fullReset)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < pins.Length; i++)
        {
            if (fullReset || pinsStillStanding[i])
            {
                pins[i].SetActive(true);
                pins[i].tag = "Pin";
                pins[i].transform.position = originalPinPositions[i];
                pins[i].transform.rotation = originalPinRotations[i];

                Rigidbody rb = pins[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                pins[i].SetActive(false);
            }
        }

        pinCounter.ResetCounter();
        roundInProgress = true;
    }

    public void UpdateScore(int pins)
    {
        score += pins;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            StartCoroutine(DecreaseTimer());
        }
    }

    private IEnumerator DecreaseTimer()
    {
        float t = timer;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

        if (roundInProgress)
        {
            roundInProgress = false;
            StartCoroutine(CompleteRound(false));
        }
    }
}
