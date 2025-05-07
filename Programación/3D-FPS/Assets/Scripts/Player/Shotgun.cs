using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Disparo")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int pellets = 8; // Cantidad de perdigones por disparo
    public float spread = 5f; // Grados de dispersión
    public float fireRate = 1f; // Tiempo entre disparos
    private float nextFireTime = 0f;
    private Animator animator;

    [Header("Munición")]
    public int maxAmmo = 8;
    public int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    [Header("Efectos")]
    public ParticleSystem muzzleFlash;
    public AudioSource shotgunSound;
    public AudioSource reloadSound;

    private Camera cam;

    private void Start()
    {
        currentAmmo = maxAmmo;
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        currentAmmo--;

        // Disparar múltiples perdigones con dispersión
        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = firePoint.forward;
            direction = Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0) * direction;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Rigidbody>().linearVelocity = direction * 50f; // Ajusta la velocidad de la bala
        }

        // Reproducir efectos
        if (muzzleFlash) muzzleFlash.Play();
        if (shotgunSound) shotgunSound.Play();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (reloadSound) reloadSound.Play();
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
