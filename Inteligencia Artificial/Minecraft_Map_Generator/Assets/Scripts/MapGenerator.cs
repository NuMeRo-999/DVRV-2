using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width = 100;
    public int height = 20;
    public int depth = 100;
    public int seed = 300000;
    public int detail = 20;

    [Range(0, 100)] public float treeProbability = 2f;

    public GameObject[] blocks;

    private HashSet<Vector3> treePositions = new HashSet<Vector3>(); // Almacena posiciones de árboles

    void Start()
    {
        seed = Random.Range(0, 1000000);
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                int localHeight = (int)(Mathf.PerlinNoise((x / 2f + seed) / detail, (z / 2f + seed) / detail) * detail);
                for (int y = 0; y < localHeight; y++)
                {
                    GameObject blockToInstantiate;

                    if (y == localHeight - 1)
                    {
                        blockToInstantiate = blocks[0]; // Grass
                    }
                    else if (y >= localHeight - 4)
                    {
                        blockToInstantiate = blocks[1]; // Dirt
                    }
                    else if (y > 0)
                    {
                        blockToInstantiate = blocks[2]; // Stone
                    }
                    else
                    {
                        blockToInstantiate = blocks[3]; // Bedrock
                    }

                    Instantiate(blockToInstantiate, new Vector3(x, y, z), Quaternion.identity, transform);

                    if (blockToInstantiate == blocks[0] && Random.Range(0, 100) < treeProbability)
                    {
                        TryCreateTree(new Vector3(x, y, z));
                    }
                }
            }
        }
    }

    public void TryCreateTree(Vector3 position)
    {
        // Verificar si la posición y sus adyacentes están libres de árboles
        if (IsPositionFree(position))
        {
            CreateTree(position);
            MarkTreePosition(position);
        }
    }

    public void CreateTree(Vector3 position)
    {
        // Crear tronco del árbol
        int logHeight = Random.Range(3, 6);
        for (int i = 1; i <= logHeight; i++)
        {
            Instantiate(blocks[4], position + Vector3.up * i, Quaternion.identity, transform);
        }

        // Crear hojas
        Vector3[] leafOffsets = new Vector3[]
        {
            new Vector3(1, logHeight - 1, 0), new Vector3(-1, logHeight - 1, 0),
            new Vector3(0, logHeight - 1, 1), new Vector3(0, logHeight - 1, -1),
            new Vector3(1, logHeight, 0), new Vector3(-1, logHeight, 0),
            new Vector3(0, logHeight, 1), new Vector3(0, logHeight, -1),
            new Vector3(0, logHeight + 1, 0), // Parte superior de las hojas
            new Vector3(1, logHeight, 1), new Vector3(-1, logHeight, -1),
            new Vector3(1, logHeight, -1), new Vector3(-1, logHeight, 1)
        };

        foreach (Vector3 offset in leafOffsets)
        {
            Instantiate(blocks[5], position + offset, Quaternion.identity, transform);
        }
    }

    private bool IsPositionFree(Vector3 position)
    {
        Vector3[] directions = {
            Vector3.zero, Vector3.left, Vector3.right, Vector3.forward, Vector3.back,
            new Vector3(1, 0, 1), new Vector3(-1, 0, -1), new Vector3(1, 0, -1), new Vector3(-1, 0, 1),
            new Vector3(1, 1, 1), new Vector3(-1, 1, -1), new Vector3(1, 1, -1), new Vector3(-1, 1, 1),
            new Vector3(1, -1, 1), new Vector3(-1, -1, -1), new Vector3(1, -1, -1), new Vector3(-1, -1, 1)
        };

        foreach (Vector3 direction in directions)
        {
            if (treePositions.Contains(position + direction))
            {
                return false;
            }
        }

        return true;
    }

    private void MarkTreePosition(Vector3 position)
    {
        Vector3[] directions = {
            Vector3.zero, Vector3.left, Vector3.right, Vector3.forward, Vector3.back,
            new Vector3(1, 0, 1), new Vector3(-1, 0, -1), new Vector3(1, 0, -1), new Vector3(-1, 0, 1),
            new Vector3(1, 1, 1), new Vector3(-1, 1, -1), new Vector3(1, 1, -1), new Vector3(-1, 1, 1),
            new Vector3(1, -1, 1), new Vector3(-1, -1, -1), new Vector3(1, -1, -1), new Vector3(-1, -1, 1)
        };

        foreach (Vector3 direction in directions)
        {
            treePositions.Add(position + direction);
        }
    }
}
