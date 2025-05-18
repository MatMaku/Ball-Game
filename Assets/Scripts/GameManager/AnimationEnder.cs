using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnder : MonoBehaviour
{
    public void EndAnimation()
    {
        Destroy(this.gameObject);
    }
}
