using Fusion;
using UnityEngine;

public class PlayerMaterialSync : NetworkBehaviour
{
    [Networked] private int materialIndex { get; set; }

    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Material[] materials;

    public override void FixedUpdateNetwork()
    {
        if (materialIndex >= 0 && materialIndex < materials.Length)
        {
            playerRenderer.material = materials[materialIndex];
        }
    }

    public void SetMaterial(Material material)
    {
        if (Object.HasStateAuthority) // Solo el propietario del objeto puede cambiar el material
        {
            int index = System.Array.IndexOf(materials, material);
            if (index != -1)
            {
                materialIndex = index;
            }
        }
    }
}
