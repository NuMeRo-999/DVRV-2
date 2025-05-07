using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManager;

public class ControlMenu : MonoBehaviour
{

    public void BotonJugar()
    {
        SceneManager.LoadScene("Game");
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    public void BotonCreditos()
    {
        SceneManager.LoadScene("Credits");
    }

    public void BotonSalirCreditos()
    {
        SceneManager.LoadScene("Menu");
    }
}

