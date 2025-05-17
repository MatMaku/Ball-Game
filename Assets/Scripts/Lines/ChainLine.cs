using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLine : MonoBehaviour
{
    public Transform startPoint; // Nave
    public Transform endPoint;   // Bola

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (startPoint && endPoint)
        {
            line.SetPosition(0, startPoint.position);
            line.SetPosition(1, endPoint.position);
        }
    }
}
