using UnityEngine;

public class PulseEmission : MonoBehaviour
{
    public Color emissionColor = Color.white;
    public float minIntensity = 1f;
    public float maxIntensity = 5f;
    public float pulseSpeed = 1f;

    private Material material;
    private float currentIntensity;
    private bool increasing = true;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
        else
        {
            Debug.LogError("PulseEmission script requires a Renderer component on the object.");
            enabled = false; 
        }

        material.SetColor("_EmissionColor", emissionColor * minIntensity);
        material.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        if (increasing)
        {
            currentIntensity += pulseSpeed * Time.unscaledDeltaTime;
            if (currentIntensity >= maxIntensity)
            {
                currentIntensity = maxIntensity;
                increasing = false;
            }
        }
        else
        {
            currentIntensity -= pulseSpeed * Time.unscaledDeltaTime;
            if (currentIntensity <= minIntensity)
            {
                currentIntensity = minIntensity;
                increasing = true;
            }
        }

        material.SetColor("_EmissionColor", emissionColor * currentIntensity);
    }
}
