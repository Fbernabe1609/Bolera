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
    public GameObject movingObject;
    private Vector3 originalMovingPosition;
    private bool isMovingObject = false;

    void Start()
    {
        originalMovingPosition = movingObject.transform.position;
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
            }
        }

        if (collision.gameObject.CompareTag("BowlingEnd"))
        {
            throwCount++;
            if (!isMovingObject)
            {
                StartCoroutine(MoveObject(movingObject, movingObject.transform.position + new Vector3(0, -0.800f, 0),
                    1f));
            }

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
        if (!isMovingObject)
        {
            StartCoroutine(MoveObject(movingObject, originalMovingPosition, 1f));
        }
    }

    IEnumerator ResetPins()
    {
        isResetting = true;
        yield return new WaitForSeconds(0.5f);
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

    IEnumerator MoveObject(GameObject obj, Vector3 targetPosition, float duration)
    {
        isMovingObject = true;
        float elapsedTime = 0;
        Vector3 startingPosition = obj.transform.position;

        while (elapsedTime < duration)
        {
            float timeFraction = elapsedTime / duration;
            obj.transform.position = Vector3.Lerp(startingPosition, targetPosition, timeFraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition;
        foreach (var pin in hitPins)
        {
            pin.SetActive(false);
        }

        isMovingObject = false;
    }
}