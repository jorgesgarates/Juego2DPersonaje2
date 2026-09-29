using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    public float velocidad = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private float movimiento;
    private bool caminando;

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
        }

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
        rb.linearVelocity = new Vector2(movimiento * velocidad, 0f);
    }
}