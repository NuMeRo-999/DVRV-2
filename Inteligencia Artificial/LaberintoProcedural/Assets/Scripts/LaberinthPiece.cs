using UnityEngine;

public class LaberinthPiece : MonoBehaviour
{

    public int x, z, chance;
    public bool n, s, e, w;

    void Start()
    {
        #region RandomNeighborGeneration
        int num = Random.Range(0, 100);
        if (num < chance && z < MapGenerator.gen.zMax - 1)
            n = true;

        num = Random.Range(0, 100);
        if (num < chance && z > 0)
            s = true;

        num = Random.Range(0, 100);
        if (num < chance && x < MapGenerator.gen.xMax - 1)
            e = true;

        num = Random.Range(0, 100);
        if (num < chance && x > 0)
            w = true;
        #endregion

        GenerateNeighbors();

        #region RandomNeighborGeneration
        if (z < MapGenerator.gen.zMax - 1 && MapGenerator.gen.map[x, z + 1] != null)
            n = true;
        if (z > 0 && MapGenerator.gen.map[x, z - 1] != null)
            s = true;
        if (x < MapGenerator.gen.xMax - 1 && MapGenerator.gen.map[x + 1, z] != null)
            e = true;
        if (x > 0 && MapGenerator.gen.map[x - 1, z] == null)
            w = true;
        #endregion

        CheckWalls();
    }

    public void GenerateNeighbors()
    {
        if (n)
            MapGenerator.gen.GenerateNextPiece(x, z + 1);
        if (s)
            MapGenerator.gen.GenerateNextPiece(x, z - 1);
        if (e)
            MapGenerator.gen.GenerateNextPiece(x + 1, z);
        if (w)
            MapGenerator.gen.GenerateNextPiece(x - 1, z);
    }

    public void CheckWalls()
    {
        if (n)
            transform.GetChild(0).gameObject.SetActive(false);

        if (s)
            transform.GetChild(1).gameObject.SetActive(false);

        if (w)
            transform.GetChild(3).gameObject.SetActive(false);

        if (e)
            transform.GetChild(2).gameObject.SetActive(false);
    }
}
