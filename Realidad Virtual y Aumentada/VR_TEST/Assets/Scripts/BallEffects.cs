// using System.Collections;
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class BallEffects : MonoBehaviour
// {
//     private Rigidbody rb;
//     private TrailRenderer trail;
//     public ParticleSystem impactEffect;
//     public AudioSource impactSound;
//     public Light glowLight;
    
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         trail = GetComponent<TrailRenderer>();
//         GetComponent<XRGrabInteractable>().selectExited.AddListener(OnSelectExited);
//         glowLight.enabled = false;
//     }

//     void Update()
//     {
//         // Activar estela si la bola se mueve rápido
//         if (rb.linearVelocity.magnitude > 2f)
//         {
//             trail.emitting = true;
//         }
//         else
//         {
//             trail.emitting = false;
//         }
//     }

//     private void OnSelectExited(SelectExitEventArgs args)
//     {
//         // Activar el brillo al soltar
//         StartCoroutine(GlowEffect());
//     }

//     private void OnCollisionEnter(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Pin"))
//         {
//             // Instanciar partículas de impacto si hay un prefab asignado
//             if (impactEffect != null)
//             {
//                 Instantiate(impactEffect, collision.contacts[0].point, Quaternion.identity);
//             }
            
//             // Reproducir sonido si hay uno asignado
//             if (impactSound != null)
//             {
//                 impactSound.Play();
//             }
//         }
//     }

//     private IEnumerator GlowEffect()
//     {
//         glowLight.enabled = true;
//         yield return new WaitForSeconds(0.5f);
//         glowLight.enabled = false;
//     }
// }
