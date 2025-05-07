using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System;
using UnityEngine;

public class Practica : MonoBehaviour
{
    public float velocity = 5f;
    public float speed = 10f;
    private string texto1 = "Buenas";
    private string texto2 = "Tardes";
    private bool isPair = false;
    public int num1 = 3;
    public int num2 = 5;

    public float countdownTime;
    public float currentTime = 10f;


    void Start()
    {
        Ejecicio1();
        Ejecicio2();
        Ejecicio3();
        Ejecicio4();
        Ejecicio5();
        Ejecicio6();
        Ejecicio7Start();
        Ejecicio8();
        Ejecicio9();

        countdownTime = currentTime;
    }

    private void Awake()
    {
        Ejecicio7Awake();
    }

    void Update()
    {
        Ejecicio10();
    }

    public void Ejecicio1()
    {
        Debug.Log(velocity);
    }

    public void Ejecicio2()
    {
        Debug.Log($"Suma: {velocity + speed}, Resta: {velocity - speed}, Multiplicación: {velocity * speed}, División: {velocity / speed}");
    }

    public void Ejecicio3()
    {
        Debug.Log(texto1 + " " + texto2);
    }

    public void Ejecicio4()
    {
        isPair = (num1 + num2) % 2 == 0;
        Debug.Log(isPair);
    }

    public void Ejecicio5()
    {
        if (DateTime.Now.DayOfWeek.ToString() == "Thursday")
        {
            Debug.Log("Es Jueves");
        }
        else
        {
            Debug.Log("No es Jueves");
        }
    }

    public void Ejecicio6()
    {
        int factorial = 1;
        for (int i = 1; i <= num1; i++)
        {
            factorial *= i;
        }
        Debug.Log("Factorial de " + num1 + " es: " + factorial);
    }

    public void Ejecicio7Start()
    {
        Debug.Log("Esto se ejecuta en la función Start");
    }

    public void Ejecicio7Awake()
    {
        Debug.Log("Esto se ejecuta en la función Awake");
    }

    public void Ejecicio8()
    {
        float deltaTime = Time.deltaTime;
        Debug.Log("Tiempo transcurrido desde el último fotograma: " + deltaTime + " segundos");
    }
    public void Ejecicio9()
    {
        StartCoroutine(PrintTimeEvery100Frames());
    }

    private IEnumerator PrintTimeEvery100Frames()
    {
        int frameCount = 0;
        while (true)
        {
            frameCount++;
            if (frameCount >= 100)
            {
                Debug.Log("Tiempo transcurrido desde el inicio: " + Time.time + " segundos");
                frameCount = 0;
            }
            yield return null;
        }
    }
    public void Ejecicio10()
    {

        if(currentTime > 0) currentTime -= Time.deltaTime;

        if(currentTime <= 0)
        {
            currentTime = countdownTime;
            int prob = UnityEngine.Random.Range(0, 100);
            int randomNumber = UnityEngine.Random.Range(0, 100);
            if(randomNumber > prob)
            {
                Debug.Log("Acierto");
            }
            else
            {
                Debug.Log("Fallo");
            }
        }
    }
}