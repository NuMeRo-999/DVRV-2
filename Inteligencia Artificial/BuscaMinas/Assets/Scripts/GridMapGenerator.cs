using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridMapGenerator : MonoBehaviour
{
    public static GridMapGenerator gen;
    public GameObject gridPiece;
    public GameObject[][] map;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject RestartButton;
    public GameObject menuButton;

    public int width;
    public int height;
    private int bombCount;
    private int revealedPieces = 0;

    void Start()
    {
        gen = this;

        width = ControlDatosJuego.Instance.Width;
        height = ControlDatosJuego.Instance.Height;
        bombCount = ControlDatosJuego.Instance.BombCount;

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        RestartButton.SetActive(false);
        menuButton.SetActive(false);

        map = new GameObject[width][];

        for (int i = 0; i < width; i++)
        {
            map[i] = new GameObject[height];
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i][j] = Instantiate(gridPiece, new Vector2(0.16f * j, 0.16f * i), Quaternion.identity, this.transform);
                map[i][j].GetComponent<Piece>().x = i;
                map[i][j].GetComponent<Piece>().y = j;
            }
        }

        for (int i = 0; i < bombCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (!map[x][y].GetComponent<Piece>().isBomb)
            {
                map[x][y].GetComponent<Piece>().isBomb = true;
            }
            else
            {
                i--;
            }
        }
    }

    public int GetBombsAround(int x, int y)
    {
        int cont = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height)
                {
                    if (map[i][j].GetComponent<Piece>().isBomb)
                    {
                        cont++;
                    }
                }
            }
        }

        return cont;
    }

    public void GameOver()
    {
        ShowAllBombs();
        RestartButton.SetActive(true);
        gameOverPanel.SetActive(true);
        menuButton.SetActive(true);
    }

    private void ShowAllBombs()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Piece piece = map[i][j].GetComponent<Piece>();
                if (piece.isBomb)
                {
                    piece.GetComponent<SpriteRenderer>().sprite = piece.bombSprite;
                }
            }
        }
    }

    public void WinGame()
    {
        ShowAllBombs();
        winPanel.SetActive(true);
        RestartButton.SetActive(true);
        menuButton.SetActive(true);
    }

    public void CheckForWin()
    {
        int flagCount = 0;
        int totalCells = width * height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Piece piece = map[i][j].GetComponent<Piece>();
                if (piece.isBomb && piece.isFlagged)
                {
                    flagCount++;
                }
            }
        }

        if (flagCount == bombCount || revealedPieces + bombCount == totalCells)
        {
            WinGame();
        }
    }

    public void IncrementRevealedPieces()
    {
        revealedPieces++;
        CheckForWin();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Debug.Log("Cargando menú");
        SceneManager.LoadScene("Menu");
    }
}
