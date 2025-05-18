using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSeparation : MonoBehaviour
{
    [Header("Separación del jugador")]
    public Transform playerTransform;
    public float distanciaMinima = 1.2f;
    public float fuerzaSeparacion = 3f;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
    }

    void FixedUpdate()
    {
        SepararDelJugador();
    }

    void SepararDelJugador()
    {
        if (playerTransform == null) return;

        Vector2 diff = (Vector2)transform.position - (Vector2)playerTransform.position;
        float distancia = diff.magnitude;

        if (distancia < distanciaMinima && distancia > 0.01f)
        {
            Vector2 direccion = diff.normalized;
            float intensidad = (distanciaMinima - distancia);

            // Mover la bola suavemente alejándose del jugador
            transform.position += (Vector3)(direccion * intensidad * fuerzaSeparacion * Time.fixedDeltaTime);
        }
    }
}
