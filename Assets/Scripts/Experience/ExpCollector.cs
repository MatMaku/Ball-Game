using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exp"))
        {
            Experience exp = other.GetComponent<Experience>();
            if (exp != null)
            {
                exp.StartCollecting(transform.root);
            }
        }
    }
}
