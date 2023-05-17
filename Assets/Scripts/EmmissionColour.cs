using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EmmissionColour : MonoBehaviour
{
    public float cycleSpeed = 1f;
    public Material targetMaterial;
    private float timeCounter = 0f;

    // Update is called once per frame
    void Update()
    {
        // Increment the time counter based on cycle speed
        timeCounter += Time.deltaTime * cycleSpeed;

        // Calculate the color using the time counter
        Color newColor = Color.HSVToRGB(timeCounter % 1f, 1f, 1f);

        // Set the new color for the spotlight
        targetMaterial.SetColor("_EmissionColor", newColor);
    }
}
