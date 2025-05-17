using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float fireRate = 2f;
    private float fireTimer;
    private Transform player;

    [Header("Entrada")]
    public float entrySpeed = 3f;
    private bool hasReachedPosition = false;
    private Vector2 targetPosition;

    public float borderOffset = 0.8f; // Qué tanto entra desde el borde visible

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Calcular targetPosition en base a la pantalla
        CalculateTargetPosition();
    }

    void Update()
    {
        RotateTowardsPlayer();

        if (!hasReachedPosition)
        {
            MoveToTargetPosition();
        }
        else
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                ShootAtPlayer();
            }
        }
    }
    void RotateTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void MoveToTargetPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, entrySpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            hasReachedPosition = true;
        }
    }

    void ShootAtPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - shootPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 2f;
    }

    void CalculateTargetPosition()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 viewportMin = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 viewportMax = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        Vector3 screenCenter = (viewportMin + viewportMax) / 2f;

        Vector2 spawnPos = transform.position;

        // Calcular dirección hacia el centro
        Vector2 directionToCenter = ((Vector2)screenCenter - spawnPos).normalized;

        // Punto en el borde más cercano del viewport
        Vector3 clampedViewportPos = cam.WorldToViewportPoint(spawnPos);
        clampedViewportPos.x = Mathf.Clamp01(clampedViewportPos.x);
        clampedViewportPos.y = Mathf.Clamp01(clampedViewportPos.y);
        Vector3 closestPointInside = cam.ViewportToWorldPoint(clampedViewportPos);

        // Aplicar el offset en la dirección hacia el centro de la pantalla
        targetPosition = (Vector2)closestPointInside + directionToCenter * borderOffset;
    }
}
