using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerLine : MonoBehaviour
{
    public Transform player;
    public float pointSpacing = 0.2f;  // Distancia entre puntos

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector3 screenPos = Input.touchCount > 0 ?
                (Vector3)Input.GetTouch(0).position :
                Input.mousePosition;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;

            DrawDashedLine(player.position, worldPos);
        }
        else
        {
            line.positionCount = 0;
        }
    }

    void DrawDashedLine(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        int points = Mathf.CeilToInt(distance / pointSpacing);
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < points; i += 2)
        {
            float t1 = (float)i / points;
            float t2 = (float)(i + 1) / points;
            positions.Add(Vector3.Lerp(start, end, t1));
            positions.Add(Vector3.Lerp(start, end, t2));
        }

        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());
    }
}
