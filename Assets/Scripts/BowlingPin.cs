using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    public bool isHit = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    void Awake()
    {
        originalPosition = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BowlingPin") || collision.gameObject.CompareTag("BowlingBall") || collision.gameObject.CompareTag("BowlingEnd"))
        {
            isHit = true;
        }
    }
    
    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        isHit = false;
    }
}
