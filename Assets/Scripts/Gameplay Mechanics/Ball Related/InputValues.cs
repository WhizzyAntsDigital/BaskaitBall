using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputValues : MonoBehaviour
{
    public static InputValues instance;
    [field: Header("Values for Ball Input")]
    [field: SerializeField] public float minimumSwipeLength { get; private set; } = 50f; 
    [field: SerializeField] public float initialAngle { get; private set; } = 45f;
    [field: SerializeField] public float forwardSpeed { get; private set; } = -8f;
    [field: SerializeField] public float upwardSpeedMultiplier { get; private set; } = 125;
    [field: SerializeField] public string uiElementTag { get; private set; } = "TouchLimit";
    [field: SerializeField] AudioSource audio;
    [field: SerializeField] List<AudioClip> whooshSounds;
    [field: SerializeField] List<AudioClip> bounceSounds;

    private void Awake()
    {
        instance = this;
    }
    public void PlayWhoosh()
    {
        audio.clip = whooshSounds[Random.Range(0, whooshSounds.Count)];
        audio.Play();
    }
    public void PlayBounce()
    {
        audio.clip = bounceSounds[Random.Range(0, whooshSounds.Count)];
        audio.Play();
    }
}
