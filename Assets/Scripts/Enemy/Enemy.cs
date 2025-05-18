using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 3;
    protected int currentHealth;
    public GameObject ExplosionPrefab;

    [Header("Experience drop")]
    public GameObject expPrefab;
    public int minExpDrop = 0;
    public int maxExpDrop = 3;
    public float expSpreadForce = 2f;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
            Die();
        }
    }

    protected virtual void Die()
    {
        int amountToDrop = Random.Range(minExpDrop, maxExpDrop + 1);

        for (int i = 0; i < amountToDrop; i++)
        {
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

            Rigidbody2D rb = exp.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * expSpreadForce, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(1);
        }
    }
}
