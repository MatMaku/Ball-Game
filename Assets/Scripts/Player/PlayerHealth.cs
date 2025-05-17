using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 4;
    private int currentHealth;

    public GameObject ExplosionPrefab;
    public GameObject[] LivesUI;

    void Start()
    {
        currentHealth = maxHealth;
        RefreshUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        RefreshUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void RefreshUI()
    {
        for (int i = 0; i < LivesUI.Length; i++)
        {
            LivesUI[i].SetActive(false);
        }

        for (int i = 0; i < currentHealth; i++)
        {
            LivesUI[i].SetActive(true);
        }
    }
}
