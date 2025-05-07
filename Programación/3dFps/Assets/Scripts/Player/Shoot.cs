using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public float shootSpeed = 10f;
    public float shootTime = 0.5f;
    private bool canShoot = false;
    public GameObject[] weapons;
    public Transform shootPosition;
    public GameObject bullet;

    private int currentWeapon = 0;
    private PlayerMouse playerMouse;

    void Start()
    {
        playerMouse = FindFirstObjectByType<PlayerMouse>();
        ChangeWeapon(0);
    }

    private void ChangeWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[index].SetActive(true);
        currentWeapon = index;
        playerMouse.SetWeapon(weapons[index].transform);
    }

    public GameObject GetCurrentWeapon()
    {
        return weapons[currentWeapon];
    }

    public void OnNext(InputValue value)
    {
        currentWeapon = (currentWeapon + 1) % weapons.Length;
        ChangeWeapon(currentWeapon);
    }

    public void OnAttack(InputValue value)
    {
        if (canShoot)
        {
            canShoot = false;
            // GameObject bullet = Instantiate(bullets[currentWeapon], shootPosition[currentWeapon].position, shootPosition[currentWeapon].rotation);
        }
    }

    IEnumerator ShootCoroutine()
    {
        canShoot = true;
        float time = 0f;
        while (time < shootTime)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.forward * shootSpeed * Time.deltaTime);
            yield return null;
        }
        Instantiate(bullet, transform.position, transform.rotation);
    }

}
