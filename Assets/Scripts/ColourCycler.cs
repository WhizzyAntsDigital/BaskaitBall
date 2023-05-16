using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourCycler : MonoBehaviour
{
    public float cycleSpeed = 1f; // Speed of color cycling
    private Light spotlight;
    private float timeCounter = 0f;

    private void Start()
    {
        spotlight = GetComponent<Light>();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        // Increment the time counter based on cycle speed
        timeCounter += Time.deltaTime * cycleSpeed;

        // Calculate the color using the time counter
        Color newColor = Color.HSVToRGB(timeCounter % 1f, 1f, 1f);

        // Set the new color for the spotlight
        spotlight.color = newColor;
    }
}
