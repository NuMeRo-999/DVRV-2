using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public void changeScene()
    {
        SceneManager.LoadScene("SC Demo Scene");
    }
}
