using UnityEngine;

public class Ejercicios : MonoBehaviour
{
  [SerializeField] GameObject Cubo2;
  int clickCount = 0;

  void Update()
  {
    Ejercicio1();
    Ejercicio2();
    Ejercicio3();
    Ejercicio4();
    Ejercicio5();
    Ejercicio6();
  }

  private void OnMouseDown()
  {
    Ejercicio7();
    Ejercicio8();
    Ejercicio9();
    Ejercicio10();
  }

  // Crea un cubo que se desplace en el eje X a una velocidad de 1 unidad por segundo. 
  // Al llegar a la posición 10, debe recolocarse en la posición 0.
  public void Ejercicio1()
  {
    transform.Translate(Vector3.right * Time.deltaTime);
    if (transform.position.x >= 10)
    {
      transform.position = new Vector3(0, 0, 0);
    }
  }

  // Haz que un cubo rote a la izquierda o a la derecha respondiendo a las pulsaciones del teclado correspondientes 
  // a los cursores ←, → o a las letras A y D.
  public void Ejercicio2()
  {
    if (Input.GetAxisRaw("Horizontal") < 0)
    {
      transform.Rotate(Vector3.up);
    }
    else if (Input.GetAxisRaw("Horizontal") > 0)
    {
      transform.Rotate(Vector3.down);
    }
  }

  // Haz que un cubo aumente la escala si mantenemos pulsada la tecla espaciadora y la disminuya si no la pulsamos.
  // La escala mínima debe ser 1 en cada eje.
  public void Ejercicio3()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      transform.localScale += Vector3.one * Time.deltaTime;
    }
    else
    {
      transform.localScale -= Vector3.one * Time.deltaTime;
    }

    if (transform.localScale.magnitude < Vector3.one.magnitude) transform.localScale = Vector3.one;
  }

  // Crea un cubo que se desplace uniformemente entre x=0 y x=10. Debe ir y volver continuamente.  
  public void Ejercicio4()
  {
    float pingPong = Mathf.PingPong(Time.time, 10);
    transform.position = new Vector3(pingPong, transform.position.y, transform.position.z);
  }

  // Crea un simple controlador de personaje que desplace un cubo en los ejes X y Z al pulsar los cursores.  
  public void Ejercicio5()
  {
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
    transform.Translate(movement * Time.deltaTime * 5.0f);
  }

  // Crea un objeto que se desplace en el eje Z una unidad al soltar la tecla espaciadora.
  public void Ejercicio6()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      transform.Translate(0, 0, transform.position.z + 1);
    }
  }

  // Crea un cubo que se destruye al clicar en él.
  private void Ejercicio7()
  {
    Destroy(gameObject);
  }

  // Crea un cubo que se destruya a los tres segundos de clicar en él.
  private void Ejercicio8()
  {
    Destroy(gameObject, 3f);
  }

  // Crea un cubo que se desactive entre tres y cinco segundos después de clicar en él.
  private void Ejercicio9()
  {
    int random = Random.Range(3, 6);
    Destroy(gameObject, random);
  }

  // Crea un cubo que cambie de nombre para indicar cuántas veces has clicado en otro cubo.
  private void Ejercicio10()
  {
    clickCount++;
    Cubo2.name = "Cubo clicado " + clickCount + " veces";
  }
}
