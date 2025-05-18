using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    private Transform target;
    private float speed = 0.5f;
    private float acceleration = 3f;
    private bool isBeingCollected = false;

    [Header("Vida útil")]
    public float lifeTime = 5f;
    public float blinkStartTime = 3f;
    private float timer = 0f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isBeingCollected && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            speed += acceleration * Time.deltaTime;
            transform.position += direction * speed * Time.deltaTime;
        }

        timer += Time.deltaTime;

        if (timer >= blinkStartTime)
        {
            float blink = Mathf.PingPong(Time.time * 5f, 1f);
            spriteRenderer.enabled = blink > 0.5f;
        }

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void StartCollecting(Transform player)
    {
        if (!isBeingCollected)
        {
            target = player;
            isBeingCollected = true;
            speed = 0.5f;
            spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExperience exp = other.GetComponent<PlayerExperience>();
            if (exp != null)
            {
                exp.AddExperience(1);
            }

            Destroy(gameObject);
        }
    }
}
