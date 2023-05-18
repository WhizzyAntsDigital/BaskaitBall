using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickeringLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 5f;

    private float randomOffset;

    private void Start()
    {
        randomOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        float flickerValue = Mathf.PerlinNoise(Time.time * flickerSpeed + randomOffset, 0);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, flickerValue);

        flickeringLight.intensity = intensity;
    }
}
