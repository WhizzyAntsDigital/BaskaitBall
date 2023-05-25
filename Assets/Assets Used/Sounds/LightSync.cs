using UnityEngine;

public class LightSync : MonoBehaviour
{
    public AudioSource audioSource; 
    public Light lightSource; 

    void Update()
    {
        float[] spectrumData = new float[256];
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

        float average = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            average += spectrumData[i]; 
        }
        average /= spectrumData.Length;

       
        float intensity = Mathf.Lerp(0f, 600f, average);
        lightSource.intensity = intensity;
    }
}
