using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BowlingBall : MonoBehaviour
{
    private Vector3 originalPosition;
    private int throwCount = 0;
    private List<GameObject> hitPins = new List<GameObject>();
    public TMP_Text scoreText;
    public AudioClip strikeSound;
    public AudioClip hitSound;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BowlingPin"))
        {
            BowlingPin pin = collision.gameObject.GetComponent<BowlingPin>();
            if (pin.isHit && !hitPins.Contains(collision.gameObject))
            {
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                hitPins.Add(collision.gameObject);
                collision.gameObject.SetActive(false);
            }
        }

        if (collision.gameObject.CompareTag("BowlingEnd"))
        {
            throwCount++;
            StartCoroutine(ResetPosition());

            if (throwCount == 2 || hitPins.Count == 12)
            {
                if (hitPins.Count == 12)
                {
                    GetComponent<AudioSource>().PlayOneShot(strikeSound);
                }

                scoreText.text = "Total pins knocked: " + hitPins.Count;
                StartCoroutine(ResetPins());
                throwCount = 0;
            }
            else
            {
                scoreText.text = "Pins knocked this throw: " + hitPins.Count;
                StartCoroutine(ResetPinsAfterDelay());
            }
        }
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(2);
        transform.position = originalPosition;
    }

    IEnumerator ResetPins()
    {
        yield return new WaitForSeconds(3);
        foreach (GameObject pin in hitPins)
        {
            pin.SetActive(true);
        }

        hitPins.Clear();
    }

    IEnumerator ResetPinsAfterDelay()
    {
        yield return new WaitForSeconds(2);
        foreach (GameObject pin in hitPins)
        {
            pin.SetActive(false);
        }

        hitPins.Clear();
    }
}