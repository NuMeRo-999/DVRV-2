using UnityEngine;

public class ControlDatosJuego : MonoBehaviour
{
    public static ControlDatosJuego Instance { get; private set; }

    private int _width;
    private int _height;
    private int _bombCount;

    public int Width
    {
        get => _width;
        set => _width = value;
    }

    public int Height
    {
        get => _height;
        set => _height = value;
    }

    public int BombCount
    {
        get => _bombCount;
        set => _bombCount = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
