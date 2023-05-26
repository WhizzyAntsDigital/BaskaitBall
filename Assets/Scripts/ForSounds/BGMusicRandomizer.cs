using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicRandomizer : MonoBehaviour
{
    [field: Header("Background Music Selector")]
    [field: SerializeField] private List<AudioClip> musicFiles;
    [field: SerializeField] private AudioSource musicSource;

    private void Start()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
        musicSource.clip = musicFiles[Random.Range(0, musicFiles.Count)];
        musicSource.Play();
    }
}
