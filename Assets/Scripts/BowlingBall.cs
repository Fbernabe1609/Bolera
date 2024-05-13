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
    public bool isResetting = false;
    public bool isBallResetting = false;
    private List<GameObject> allPins = new List<GameObject>();

    void Start()
    {
        originalPosition = transform.position;
        scoreText.text = "";
        GameObject[] pinObjects = GameObject.FindGameObjectsWithTag("BowlingPin");
        foreach (GameObject pinObject in pinObjects)
        {
            allPins.Add(pinObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isResetting || isBallResetting)
        {
            return;
        }

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

            if (throwCount == 2 || hitPins.Count == 10)
            {
                if (hitPins.Count == 10 && throwCount == 1)
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
            }
        }
    }

    IEnumerator ResetPosition()
    {
        isBallResetting = true;
        yield return new WaitForSeconds(2);
        transform.position = originalPosition;
        isBallResetting = false;
    }

    IEnumerator ResetPins()
    {
        isResetting = true;
        yield return new WaitForSeconds(5);
        for (int i = 0; i < allPins.Count; i++)
        {
            GameObject pin = allPins[i];
            pin.SetActive(true);
            BowlingPin bowlingPin = pin.GetComponent<BowlingPin>();
            bowlingPin.ResetPosition();
        }

        scoreText.text = "";
        hitPins.Clear();
        isResetting = false;
    }
}