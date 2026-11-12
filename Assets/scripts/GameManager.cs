using UnityEngine;
using System.Collections.Generic; // HashSet
using TMPro;  

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;

    private GameObject[] pins;
    private Vector3[] startPositions;
    private Quaternion[] startRotations;

    private HashSet<GameObject> hitPins = new HashSet<GameObject>();
    private int score;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");

        startPositions = new Vector3[pins.Length];
        startRotations = new Quaternion[pins.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            startPositions[i] = pins[i].transform.position;
            startRotations[i] = pins[i].transform.rotation;
        }

        scoreText.text = score.ToString();
    }

    public void HitPin(GameObject pin)
    {
        if (!hitPins.Add(pin)) return;

        score++;
        scoreText.text = score.ToString();

        if (hitPins.Count == pins.Length)
        {
            ResetPins();
        }
    }

    private void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Rigidbody pinRb = pins[i].GetComponent<Rigidbody>();

            pinRb.linearVelocity = Vector3.zero;
            pinRb.angularVelocity = Vector3.zero;
            pinRb.position = startPositions[i];
            pinRb.rotation = startRotations[i];
        }

        hitPins.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
