using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class End : MonoBehaviour
{
    public GameObject canvas;
    public float duration = 1f;

    private void Start()
    {
        canvas.SetActive(false);
    }

    private IEnumerator FadeInCanvas(GameObject canvas, float duration)
    {
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvas.AddComponent<CanvasGroup>();
        }

        canvas.SetActive(true);
        canvasGroup.alpha = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // Asegura que el alpha sea 1 al final
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInCanvas(canvas, duration));
        }
    }
}
