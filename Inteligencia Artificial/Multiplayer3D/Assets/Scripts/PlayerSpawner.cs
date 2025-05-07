using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private Material[] materials;
    public GameObject PlayerPrefab;

    [Header("Spawn Range")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    private static List<Material> availableMaterials = new List<Material>();
    private static Material assignedMaterial;

    private void Start()
    {
        // Inicializar la lista de materiales disponibles solo una vez
        if (availableMaterials.Count == 0)
        {
            availableMaterials = new List<Material>(materials);
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log($"Player joined: {player} | LocalPlayer: {Runner.LocalPlayer}");
        if (player == Runner.LocalPlayer)
        {
            float randomX = UnityEngine.Random.Range(minX, maxX);
            float randomZ = UnityEngine.Random.Range(minZ, maxZ);

            Vector3 spawnPosition = new Vector3(randomX, 1, randomZ);
            NetworkObject networkObject = Runner.Spawn(PlayerPrefab, spawnPosition, Quaternion.identity, player);
            GameObject newPlayer = networkObject.gameObject;

            AssignMaterial(newPlayer);
        }
    }

    private void AssignMaterial(GameObject player)
    {
        if (Runner.IsServer)
        {
            // Si es el primer jugador, asigna un material aleatorio
            if (assignedMaterial == null && availableMaterials.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, availableMaterials.Count);
                assignedMaterial = availableMaterials[index];
                availableMaterials.RemoveAt(index);
            }

            // Sincronizar el material con todos los jugadores
            player.GetComponent<NetworkObject>().GetComponent<PlayerMaterialSync>().SetMaterial(assignedMaterial);
        }
    }
}
