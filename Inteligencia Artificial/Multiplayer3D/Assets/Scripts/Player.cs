 using Fusion;
using UnityEngine;
using System.Linq;

public class Player : NetworkBehaviour
{
    [Networked] public int ColorIndex { get; set; } // Sync player color across network
    public Material[] playerMaterials;
    public Camera Camera;

    void Start()
    {
        int randomIndex = Random.Range(0, playerMaterials.Length);
        ColorIndex = randomIndex;
        // playerMaterials = playerMaterials.Where((val, idx) => idx != randomIndex).ToArray();
    }

    public override void Render()
    {
        GetComponent<Renderer>().material = playerMaterials[ColorIndex]; // Apply correct material
    }
    
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            GetComponent<PlayerMovement>().SetCamera(Camera);
        }
    }
}