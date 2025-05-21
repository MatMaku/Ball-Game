using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBoss : Enemy
{
    [Header("Shooter Settings")]
    public GameObject projectilePrefab;
    public Transform[] shootPoints;
    public float fireRate = 2f;
    private float fireTimer;
    private int nextShootIndex = 0;
    private Transform player;

    [Header("Laser Settings")]
    public GameObject laserPrefab;
    public float laserCooldown = 6f;
    private float laserTimer;

    [Header("Movement Settings")]
    public Vector2 startPosition;
    public Vector2 finalPosition;
    public float entrySpeed = 3f;
    private bool hasReachedPosition = false;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1f, 0.3f, 0.3f, 1f);
    public float damageFlashDuration = 0.1f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = startPosition;
        transform.rotation = new Quaternion(0,0,180,0);

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (!hasReachedPosition)
        {
            MoveToFinalPosition();
        }
        else
        {
            HandleRegularShooting();
            HandleLaser();
        }
    }

    void MoveToFinalPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, finalPosition, entrySpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, finalPosition) < 0.1f)
        {
            hasReachedPosition = true;
        }
    }

    void HandleRegularShooting()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            ShootAtPlayer();
        }
    }

    void HandleLaser()
    {
        laserTimer += Time.deltaTime;
        if (laserTimer >= laserCooldown)
        {
            laserTimer = 0f;
            StartCoroutine(FireLaser());
        }
    }

    void ShootAtPlayer()
    {
        if (player == null || shootPoints.Length == 0) return;

        Transform shootPoint = shootPoints[nextShootIndex];
        Vector2 direction = (player.position - shootPoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 2f;

        nextShootIndex = (nextShootIndex + 1) % shootPoints.Length;
    }

    IEnumerator FireLaser()
    {
        if (laserPrefab == null) yield break;

        Instantiate(laserPrefab, this.transform.position, Quaternion.identity, transform);
    }

    public override void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (spriteRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(DamageFlash());
        }

        if (currentHealth <= 0)
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }
}