using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Data")]
    public int maxHealth = 4;
    private int currentHealth;
    public float invincibilityDuration = 2f;
    public float flashInterval = 0.1f;

    [Header("Prefabs & UI")]
    public GameObject ExplosionPrefab;
    public GameObject[] LivesUI;
    public GameObject GameOverUI;

    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameOverUI.SetActive(false);

        RefreshUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        RefreshUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    void Die()
    {
        Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
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

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
