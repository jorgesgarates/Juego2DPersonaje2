
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bala : MonoBehaviour
{
    [Header("Configuracion de Bala")]
    public float velocidad = 10f;
    public float tiempoDeVida = 3f;

    private Rigidbody2D rb;
    private float direccion = 1f;
    private bool disparada = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.angularVelocity = 0f;

        rb.collisionDetectionMode =
            CollisionDetectionMode2D.Continuous;

        Destroy(gameObject, tiempoDeVida);
    }

    public void Disparar(float nuevaDireccion)
    {
        // DETERMINAR DIRECCION
        direccion = nuevaDireccion < 0f ? -1f : 1f;

        disparada = true;

        // APLICAR VELOCIDAD HORIZONTAL
        rb.linearVelocity = new Vector2(
            direccion * velocidad,
            0f
        );

        Debug.Log(
            direccion < 0f
                ? "BALA MOVIENDOSE A LA IZQUIERDA"
                : "BALA MOVIENDOSE A LA DERECHA"
        );
    }

    void FixedUpdate()
    {
        if (!disparada)
            return;

        // MANTENER TRAYECTORIA HORIZONTAL
        rb.linearVelocity = new Vector2(
            direccion * velocidad,
            0f
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
