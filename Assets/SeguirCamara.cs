using UnityEngine;

public class SeguirCamara : MonoBehaviour
{
    public Transform jugador;
    public float suavidad = 5f;

    void LateUpdate()
    {
        if (jugador != null)
        {
            Vector3 destino = new Vector3(
                jugador.position.x,
                transform.position.y,
                transform.position.z
            );

            transform.position = Vector3.Lerp(
                transform.position,
                destino,
                suavidad * Time.deltaTime
            );
        }
    }
}