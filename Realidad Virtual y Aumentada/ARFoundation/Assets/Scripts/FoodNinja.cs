using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodNinja : MonoBehaviour
{

    public GameObject[] food;
    public GameObject bullet;
    public GameObject ARCamera;
    public float speed = 2000f;

    [SerializeField] private ARPlaneManager arPlaneManager; 
    private List<ARPlane> planes = new List<ARPlane>();
    private bool empezarGenerar = false;
    public float alturaPlanoDetectado;

    public Sprite buttonSprite;
    public Sprite buttonSpritePressed;
    public Image Button;

    public GameObject[] healthUI;
    public Sprite emptyHeart;
    private int health = 3;

    private int points = 0;
    public TextMeshProUGUI pointsUI;

    public GameObject losePanel;

    void Start()
    {
        losePanel.SetActive(false);
        InvokeRepeating("SpawnFood", 1f, 2f);
        pointsUI.text = "Puntos: " + points.ToString();
    }

    void Update()
    {
      
        if (empezarGenerar)
        {
            GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject f in food)
            {
                if (f.transform.position.y < alturaPlanoDetectado)
                {
                    Destroy(f);
                    lostPoints(10);
                }
            }
        }
    }

    void SpawnFood()
    {
        if(empezarGenerar)
        {
            int index = Random.Range(0, food.Length);
            Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 10f, 4f);
            Instantiate(food[index], spawnPosition, Quaternion.identity);
        }
    }

    public void Shoot()
    {
        if (health == 0) return;
        GameObject bullet = Instantiate(this.bullet, ARCamera.transform.position, Quaternion.Euler(ARCamera.transform.rotation.eulerAngles.x, ARCamera.transform.rotation.eulerAngles.y + 90, ARCamera.transform.rotation.eulerAngles.z));
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddForce(ARCamera.transform.forward * speed);
    }
    
    private void OnEnable(){
        arPlaneManager.planesChanged += PlanesFound;
    }

    private void OnDisable(){
        arPlaneManager.planesChanged -= PlanesFound;
    }

    private void PlanesFound(ARPlanesChangedEventArgs datosPlanos){
        if(datosPlanos.added != null && datosPlanos.added.Count > 0){
            planes.AddRange(datosPlanos.added);
        }

        foreach(ARPlane plane in planes){
            if(plane.extents.x * plane.extents.y >= 1){
                alturaPlanoDetectado = plane.center.y;
                DetenerDeteccionPlanos();
            }
        }
    }

    public void DetenerDeteccionPlanos(){
        arPlaneManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        
        foreach (ARPlane plane in planes)
        {
            plane.gameObject.SetActive(false);
        }

        empezarGenerar = true;
    }

    public void takeDamage()
    {
        if (health > 0)
        {
            health -= 1;
            healthUI[health].GetComponent<Image>().sprite = emptyHeart;
        }

        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        losePanel.SetActive(true);
    }

    public void addPoints(int points)
    {
        if (health == 0) return;
        this.points += points;
        Debug.Log("Puntos añadidos: " + points.ToString());
        pointsUI.text = "Puntos: " + this.points.ToString();
    }

    public void lostPoints(int points)
    {
        if (health == 0) return;
        this.points -= points;
        pointsUI.text = "Puntos: " + this.points.ToString();
    }
    
}
