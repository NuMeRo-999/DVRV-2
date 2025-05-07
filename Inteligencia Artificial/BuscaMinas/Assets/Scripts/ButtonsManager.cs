using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public void FacilButton()
    {
        Debug.Log("FacilButton");
        ControlDatosJuego.Instance.Width = 8;
        ControlDatosJuego.Instance.Height = 8;
        ControlDatosJuego.Instance.BombCount = 10;
        SceneManager.LoadScene("Juego");
    }

    public void MedioButton()
    {
        ControlDatosJuego.Instance.Width = 16;
        ControlDatosJuego.Instance.Height = 16;
        ControlDatosJuego.Instance.BombCount = 40;
        SceneManager.LoadScene("Juego");
    }

    public void DificilButton()
    {
        ControlDatosJuego.Instance.Width = 30;
        ControlDatosJuego.Instance.Height = 30;
        ControlDatosJuego.Instance.BombCount = 99;
        SceneManager.LoadScene("Juego");
    }
}
