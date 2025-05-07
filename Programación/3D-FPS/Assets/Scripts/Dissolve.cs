using System.Collections;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public Material dissolveMaterial; // Asigna el material en el inspector

    void Start()
    {
        dissolveMaterial.SetFloat("_Dissolve", 0f);
        Invoke("StartDissolve", 3f);
        Destroy(gameObject, 5f);
    }

    public void StartDissolve()
    {
        StartCoroutine(DissolveCoroutine());
    }

    private IEnumerator DissolveCoroutine()
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime / 2f;
            dissolveMaterial.SetFloat("_Dissolve", time);
            yield return null;
        }
    }
}
