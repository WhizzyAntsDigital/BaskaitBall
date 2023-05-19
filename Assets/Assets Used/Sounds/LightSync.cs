using UnityEngine;

public class LightSync : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the audio source playing the song
    public Light lightSource; // Reference to the light source

    void Update()
    {
        float[] spectrumData = new float[256];
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular); // Get the spectrum data from the audio source

        float average = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            average += spectrumData[i]; // Calculate the average amplitude
        }
        average /= spectrumData.Length;

        // Adjust the light intensity based on the average amplitude
        float intensity = Mathf.Lerp(0f, 300f, average); // Adjust the range (0f - 10f) based on your desired effect
        lightSource.intensity = intensity;
    }
}
