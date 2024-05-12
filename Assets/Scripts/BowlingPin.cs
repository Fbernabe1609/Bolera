using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    public bool isHit = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BowlingPin") || collision.gameObject.CompareTag("BowlingBall"))
        {
            isHit = true;
        }
    }
}
