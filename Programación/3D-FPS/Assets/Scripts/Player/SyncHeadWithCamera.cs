using UnityEngine;
using Unity.Cinemachine;

public class SyncHeadWithCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera cinemachineCam; // Referencia a la cámara de Cinemachine
    [SerializeField] private Transform head; // Objeto Head que seguirá a la cámara

    private void LateUpdate()
    {
        if (cinemachineCam == null || head == null) return;

        // Sincronizar la posición del Head con la cámara
        head.position = cinemachineCam.transform.position;

        // Sincronizar la rotación del Head con la cámara
        head.rotation = cinemachineCam.transform.rotation;
    }
}