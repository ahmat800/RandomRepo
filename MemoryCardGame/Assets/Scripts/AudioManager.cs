using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundMusicSource;
    public AudioSource mainAudioSource;

    public AudioClipCollection audioClipCollection;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayMusic("MemoryCardMainMusic");
    }

    public void PlaySound(string clipName)
    {
        mainAudioSource.volume = 1;
        mainAudioSource.PlayOneShot(audioClipCollection.GetAudioClip(clipName));
    }

    public void PlayMusic(string musicName)
    {
        PlayBackgroundMusic(Resources.Load<AudioClip>($"Audio/BackgroundMusic/{musicName}"), 1);
    }

    private void PlayBackgroundMusic(AudioClip backgroundMusic, float targetVolume, float fadeTime = 0.3f)
    {

        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.volume = targetVolume;
        backgroundMusicSource.Play();
    }
}
