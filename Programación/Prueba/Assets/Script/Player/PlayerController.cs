using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed = 5;
    public float jumpForce = 5;
    public bool isGrounded;
    private Rigidbody2D rb;
    private Transform transform;
    private Animator anim;
    public AudioSource jumpSound;
    public bool dead = false;
    public GameObject prefab;
    public ControlHud ControlHud;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (dead) return;

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetAxis("Horizontal") > 0)
        {
            anim.SetBool("walking", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            anim.SetBool("walking", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            anim.SetTrigger("jump");
            jumpSound.Play();
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            anim.SetBool("isGrounded", isGrounded);
        }
        else
        {
            anim.SetBool("isGrounded", isGrounded);
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }

    public bool IsGrounded()
    {
        // Mostrar el raycast
        Debug.DrawRay(transform.position + new Vector3(0, -2f, 0), Vector2.down * 2f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, -2f, 0), Vector2.down, .5f);
        return hit.collider != null;
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy")
        {
            if (ControlHud.vida > 1)
            {
                ControlHud.UpdateLife(ControlHud.vida - 1);
                ControlHud.vida -= 1;
            }
            else
            {
                ControlHud.UpdateLife(ControlHud.vida - 1);
                dead = true;
                StartCoroutine(CourrutineGameOver());
            }
        }
        isGrounded = true;
    }

    IEnumerator CourrutineGameOver()
    {
        yield return new WaitForSeconds(1);
        FinJuego();
    }

    private void FinJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }
}
