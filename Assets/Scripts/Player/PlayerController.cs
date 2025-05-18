using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Velocidad movimiento")]
    public float moveSpeed = 5f; 
    private Rigidbody2D rb;
    private Vector2 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
            targetPosition = new Vector2(worldPos.x, worldPos.y);
        }
    #if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = new Vector2(worldPos.x, worldPos.y);
        }
    #endif
    }

    void FixedUpdate()
    {
        Vector2 newPosition = Vector2.Lerp(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        Vector2 direction = targetPosition - rb.position;
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }
}
