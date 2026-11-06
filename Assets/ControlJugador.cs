
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class ControlJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;

    [Header("Disparo")]
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float distanciaSalida = 1.2f;
    public float duracionAnimacionDisparo = 0.25f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 escalaInicial;

    private float movimiento = 0f;
    private float direccion = 1f;
    private float finAnimacionDisparo = 0f;
    private string animacionActual = "";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        movimiento = 0f;

        if (Keyboard.current == null)
            return;

        // MOVIMIENTO IZQUIERDA
        bool izquierda =
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed;

        // MOVIMIENTO DERECHA
        bool derecha =
            Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed;

        if (izquierda && !derecha)
        {
            movimiento = -1f;
        }
        else if (derecha && !izquierda)
        {
            movimiento = 1f;
        }

        if (movimiento != 0f)
        {
            Orientar(movimiento);
        }

        // SALTAR CON ESPACIO
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Saltar();
        }

        // DISPARAR IZQUIERDA CON Q
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Disparar(-1f);
        }

        // DISPARAR DERECHA CON F
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Disparar(1f);
        }

        ActualizarAnimacion();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );
    }

    void Orientar(float nuevaDireccion)
    {
        direccion = nuevaDireccion < 0f ? -1f : 1f;

        transform.localScale = new Vector3(
            Mathf.Abs(escalaInicial.x) * direccion,
            escalaInicial.y,
            escalaInicial.z
        );
    }

    void Saltar()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            fuerzaSalto
        );
    }

    void Disparar(float direccionDisparo)
    {
        if (balaPrefab == null)
        {
            Debug.LogError("No se asigno Bala Prefab.");
            return;
        }

        Orientar(direccionDisparo);

        // POSICION DE SALIDA DE LA BALA
        Vector3 posicionBala = transform.position;

        posicionBala.x +=
            direccionDisparo * distanciaSalida;

        if (puntoDisparo != null)
        {
            posicionBala.y = puntoDisparo.position.y;
        }

        // CREAR PROYECTIL
        GameObject nuevaBala = Instantiate(
            balaPrefab,
            posicionBala,
            Quaternion.identity
        );

        // EVITAR COLISIONES CON EL JUGADOR
        Collider2D[] colisionesBala =
            nuevaBala.GetComponentsInChildren<Collider2D>();

        Collider2D[] colisionesJugador =
            GetComponentsInChildren<Collider2D>();

        foreach (Collider2D cb in colisionesBala)
        {
            foreach (Collider2D cj in colisionesJugador)
            {
                Physics2D.IgnoreCollision(cb, cj, true);
            }
        }

        // ASIGNAR DIRECCION DEL DISPARO
        Bala bala = nuevaBala.GetComponent<Bala>();

        if (bala != null)
        {
            bala.Disparar(direccionDisparo);
        }
        else
        {
            Debug.LogError(
                "El prefab Bala necesita el script Bala.cs."
            );

            Destroy(nuevaBala);
            return;
        }

        // ANIMACION DE DISPARO
        finAnimacionDisparo =
            Time.time + duracionAnimacionDisparo;

        if (animator != null &&
            animator.HasState(
                0,
                Animator.StringToHash("Base Layer.Disparar")
            ))
        {
            animator.Play("Disparar", 0, 0f);
            animacionActual = "Disparar";
        }

        Debug.Log(
            direccionDisparo < 0f
                ? "DISPARO HACIA LA IZQUIERDA"
                : "DISPARO HACIA LA DERECHA"
        );
    }

    void ActualizarAnimacion()
    {
        if (animator == null)
            return;

        if (Time.time < finAnimacionDisparo &&
            animator.HasState(
                0,
                Animator.StringToHash("Base Layer.Disparar")
            ))
        {
            return;
        }

        string nuevaAnimacion =
            movimiento != 0f ? "Caminar" : "Idle";

        if (animacionActual != nuevaAnimacion)
        {
            animator.Play(nuevaAnimacion, 0, 0f);
            animacionActual = nuevaAnimacion;
        }
    }
}
