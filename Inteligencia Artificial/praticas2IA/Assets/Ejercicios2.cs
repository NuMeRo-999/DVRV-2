using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Ejercicios2 : MonoBehaviour
{
    public GameObject prefab;
    public List<Vector3> points = new List<Vector3>();
    public GameObject[] objects;

    void Start()
    {
        // Ejercicio1();
        // Ejercicio2();
        // Ejercicio3();
        // Ejercicio4();
        // Ejercicio5();
        // Ejercicio6();
        Ejercicio10();
    }

    void Update()
    {
        // Ejercicio7();
        // Ejercicio8();
        // Ejercicio9();
        Ejercicio10_2();
    }

    void Ejercicio1()
    {
        int sum = 0;
        for (int i = 0; i < 500; i++)
        {
            sum += i * 2;
        }
        Debug.Log("Suma de los primeros 500 números pares: " + sum);
    }

    void Ejercicio2()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(i, 0, 0);
        }
    }

    void Ejercicio3()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    void Ejercicio4()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
    }

    void Ejercicio5()
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    GameObject layerPrefab = (y == 0) ? prefab : ((y == 1) ? prefab : prefab);
                    Instantiate(layerPrefab, new Vector3(x + 5, y, z), Quaternion.identity);
                }
            }
        }
    }

    void Ejercicio6()
    {
        int baseSize = 5;
        for (int y = 0; y < baseSize; y++)
        {
            for (int x = 0; x < baseSize - y; x++)
            {
                for (int z = 0; z < baseSize - y; z++)
                {
                    Instantiate(prefab, new Vector3(x - (baseSize - y) / 2f, y, z - (baseSize - y) / 2f), Quaternion.identity);
                }
            }
        }
    }

    void Ejercicio7()
    {
        if (points.Count > 1)
        {
            StartCoroutine(MoveBetweenPoints(points));
        }
    }

    IEnumerator MoveBetweenPoints(List<Vector3> points)
    {
        int currentIndex = 0;
        while (true)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = points[currentIndex];
            float elapsedTime = 0f;
            float journeyTime = 2f;

            while (elapsedTime < journeyTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / journeyTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            currentIndex = (currentIndex + 1) % points.Count;
        }
    }

    void Ejercicio8()
    {
        if (points.Count > 1)
        {
            StartCoroutine(MoveToRandomPoint(points));
        }
    }

    IEnumerator MoveToRandomPoint(List<Vector3> points)
    {
        while (true)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = points[Random.Range(0, points.Count)];
            float elapsedTime = 0f;
            float journeyTime = 2f;

            while (elapsedTime < journeyTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / journeyTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }
    }

    void Ejercicio9()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("desaparecer");
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);
            }
        }
    }


    void Ejercicio10()
    {
        objects = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            GameObject obj = Instantiate(prefab, randomPosition, Quaternion.identity);
            objects[i] = obj;
        }

        
    }

    void Ejercicio10_2()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
                foreach (GameObject obj in objects)
                {
                    if (obj.transform.position.x > 0)
                    {
                        Destroy(obj);
                    }
                }
        }
    }
}
