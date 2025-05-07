using UnityEngine;
using UnityEngine.SceneManagement;


public class Button : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
