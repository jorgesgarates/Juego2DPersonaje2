using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 10f;
    public float tiempoDeVida = 3f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, tiempoDeVida);
    }

    public void Disparar(float direccion)
    {
        rb.linearVelocity = new Vector2(direccion * velocidad, 0f);
    }
}