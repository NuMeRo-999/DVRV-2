using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator gen;
    public int xMax, zMax;
    public GameObject piece, laberinthPiece;
    public GameObject[,] map;
    public int limit;

    void Start()
    {
        gen = this;
        map = new GameObject[xMax, zMax];
        // StartCoroutine("GenMapBasic");
        // StartCoroutine(GenMapMedium(0, 0));
        GenerateFirstFloor();
    }

    public IEnumerator GenMapBasic()
    {
        for (int x = 0; x < xMax; x++)
        {
            for (int z = 0; z < zMax; z++)
            {
                if (Random.Range(0, 100) < 50)
                {
                    Instantiate(piece, new Vector3(x * 5, 0, z * 5), Quaternion.identity);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }

    public IEnumerator GenMapMedium(int x, int z)
    {
        limit--;

        Transform newPiece = Instantiate(piece, new Vector3(x, 0, z), Quaternion.identity).transform;
        yield return new WaitForEndOfFrame();

        if (limit > 0)
        {
            bool complete = false;
            int count = 0;
            while (!complete && count < 50)
            {
                count++;
                int num = Random.Range(0, 100);
                if (num < 25 && !Physics.Raycast(newPiece.position, newPiece.forward, 6))
                { StartCoroutine(GenMapMedium(x, z + 5)); complete = true; }
                else if (num < 50 && !Physics.Raycast(newPiece.position, newPiece.forward * -1, 6))
                { StartCoroutine(GenMapMedium(x, z - 5)); complete = true; }
                else if (num < 75 && !Physics.Raycast(newPiece.position, newPiece.right, 6))
                { StartCoroutine(GenMapMedium(x + 5, z)); complete = true; }
                else if (num < 90 && !Physics.Raycast(newPiece.position, newPiece.right * -1, 6))
                { StartCoroutine(GenMapMedium(x - 5, z)); complete = true; }
            }
        }
    }

    public void GenerateFirstFloor()
    {
        LaberinthPiece newPiece = Instantiate(laberinthPiece, new Vector3((xMax / 2) * 5, 0, (zMax / 2) * 5), Quaternion.identity).GetComponent<LaberinthPiece>();
        newPiece.n = true;
        newPiece.s = true;
        newPiece.e = true;
        newPiece.w = true;

        newPiece.x = xMax / 2;
        newPiece.z = zMax / 2;
        map[xMax / 2, zMax / 2] = newPiece.gameObject;
    }

    public void GenerateNextPiece(int x, int z)
    {
        if (map[x, z] == null)
        {
            LaberinthPiece newPiece = Instantiate(laberinthPiece, new Vector3(x * 5, 0, z * 5), Quaternion.identity).GetComponent<LaberinthPiece>();
            newPiece.x = x;
            newPiece.z = z;
            map[x, z] = newPiece.gameObject;
        }
    }
}
