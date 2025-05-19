using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSeparation : MonoBehaviour
{
    [Header("Separación del jugador")]
    public Transform playerTransform;
    public float distanciaMinimaP = 1.2f;
    public float fuerzaSeparacionP = 3f;

    [Header("Separación de otras bolas")]
    public float distanciaMinimaB = 0.5f;
    public float fuerzaSeparacionB = 2f;

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
        SepararDeOtrasBolas();
    }

    void SepararDelJugador()
    {
        if (playerTransform == null) return;

        Vector2 diff = (Vector2)transform.position - (Vector2)playerTransform.position;
        float distancia = diff.magnitude;

        if (distancia < distanciaMinimaP && distancia > 0.01f)
        {
            Vector2 direccion = diff.normalized;
            float intensidad = (distanciaMinimaP - distancia);

            // Mover la bola suavemente alejándose del jugador
            transform.position += (Vector3)(direccion * intensidad * fuerzaSeparacionP * Time.fixedDeltaTime);
        }
    }

    void SepararDeOtrasBolas()
    {
        GameObject[] bolas = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject otraBola in bolas)
        {
            if (otraBola == this.gameObject) continue;

            Vector2 diff = (Vector2)transform.position - (Vector2)otraBola.transform.position;
            float distancia = diff.magnitude;

            if (distancia < distanciaMinimaB && distancia > 0.01f)
            {
                Vector2 direccion = diff.normalized;
                float intensidad = (distanciaMinimaB - distancia);

                transform.position += (Vector3)(direccion * intensidad * fuerzaSeparacionB * Time.fixedDeltaTime);
            }
        }
    }
}
