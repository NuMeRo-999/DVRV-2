using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagerDialog : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
