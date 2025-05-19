using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [Header("Values")]
    [Range(0.5f, 3f)] public float Tamaño = 0.5f;
    [Range(0.8f, 3f)] public float Longitud = 1.5f;
    [Range(1f, 8f)] public float Peso = 3f;

    [Header("Anclaje")]
    public GameObject StartPoint;
    public GameObject Ball;

    private ChainLine Chain;
    private Transform BallTransform;
    private Rigidbody2D BallRb;
    private DistanceJoint2D BallDistanceJoint;

    private void Start()
    {
        Chain = GetComponentInChildren<ChainLine>();
        BallTransform = Ball.GetComponent<Transform>();
        BallRb = Ball.GetComponent<Rigidbody2D>();
        BallDistanceJoint = Ball.GetComponent<DistanceJoint2D>();

        BallDistanceJoint.connectedBody = StartPoint.GetComponent<Rigidbody2D>();
        Chain.startPoint = StartPoint.GetComponent<Transform>();
        Chain.endPoint = BallTransform;

        ActualizarValores();
    }

    public void ActualizarValores()
    {
        //Tamaño
        Ball.transform.localScale = new Vector3(Tamaño, Tamaño, Ball.transform.localScale.z);

        //Longitud
        BallDistanceJoint.distance = Longitud;

        //Peso
        BallRb.drag = Peso;
    }
}
