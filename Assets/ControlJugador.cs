using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;
    public GameObject balaPrefab;
    public Transform puntoDisparo;

    private Rigidbody2D rb;
    private Animator animator;
    private float movimiento;
    private bool caminando;
    private float direccion = 1f;
    private bool enSuelo;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movimiento = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                movimiento = -1f;

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                movimiento = 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Saltar();

            if (Keyboard.current.fKey.wasPressedThisFrame)
                Disparar();
        }

        if (movimiento != 0f)
            direccion = movimiento > 0f ? 1f : -1f;

        bool seEstaMoviendo = movimiento != 0f;

        if (seEstaMoviendo != caminando)
        {
            animator.Play(seEstaMoviendo ? "Caminar" : "Idle");
            caminando = seEstaMoviendo;
        }

        transform.localScale = seEstaMoviendo
            ? new Vector3(1.2f, 1.2f, 1f)
            : Vector3.one;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);
    }

    void Saltar()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
    }

    void Disparar()
    {
        GameObject nuevaBala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        nuevaBala.GetComponent<Bala>().Disparar(direccion);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Suelo")
            enSuelo = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Suelo")
            enSuelo = false;
    }
}