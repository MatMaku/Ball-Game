using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;  // Velocidad de seguimiento
    private Rigidbody2D rb;

    private Vector2 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    void Update()
    {
        // Detectar el toque o click
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
            targetPosition = new Vector2(worldPos.x, worldPos.y);
        }
#if UNITY_EDITOR  // Para probar con mouse en la PC
        if (Input.GetMouseButton(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = new Vector2(worldPos.x, worldPos.y);
        }
#endif
    }

    void FixedUpdate()
    {
        // Movimiento suave hacia el dedo
        Vector2 newPosition = Vector2.Lerp(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Rotar hacia donde se mueve
        Vector2 direction = targetPosition - rb.position;
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }
}
