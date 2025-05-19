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
        if (Time.timeScale > 0.1f)
        {
            Vector2 newTarget = targetPosition;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                newTarget = new Vector2(worldPos.x, worldPos.y);
            }

            #if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newTarget = new Vector2(worldPos.x, worldPos.y);
            }
            #endif
            Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

            newTarget.x = Mathf.Clamp(newTarget.x, min.x, max.x);
            newTarget.y = Mathf.Clamp(newTarget.y, min.y, max.y);

            targetPosition = newTarget;
        }
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
