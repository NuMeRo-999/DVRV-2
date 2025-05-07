using UnityEngine;

public class BlockTexture : MonoBehaviour
{
    MeshFilter meshFilter;
    Mesh mesh;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        Vector2[] uvs = mesh.uv;

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i].x += Random.Range(-0.1f, 0.1f);
            uvs[i].y += Random.Range(-0.1f, 0.1f);
        }

        mesh.uv = uvs;
        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
