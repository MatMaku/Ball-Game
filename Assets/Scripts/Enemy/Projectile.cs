using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí deberías llamar a tu sistema de daño del jugador
            Debug.Log("Jugador impactado!");
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ball")) // Tag que le pongas a la bola
        {
            Debug.Log("Bala bloqueada!");
            Destroy(gameObject);
        }
    }
}
