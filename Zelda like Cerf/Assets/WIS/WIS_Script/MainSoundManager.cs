using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class MainSoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public Music[] Musics;

    public static MainSoundManager instance;
    private AudioSource[] Audiosources;


    public Music CurrentMusic;

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }


        //On instancie les différents audiospurces pour chaque sons
        foreach (Sound s in sounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume * AudioManager.SoundEffectsVolume;

            s.source.loop = s.loop;
        }

        foreach (Music s in Musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume * MusicManager.MusicVolume;

            s.source.loop = s.loop;
        }


    }


    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
    }


    public void PlayMusic()
    {
        CurrentMusic = Array.Find(Musics, s => s.name == "ThemeV1");
        StartCoroutine(FadeIn(CurrentMusic.source, 1, CurrentMusic.volume));
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float FinalVolume)
    {
        audioSource.volume = 0;
        audioSource.Play();

        FinalVolume = FinalVolume * MusicManager.MusicVolume;

        while (audioSource.volume < FinalVolume)
        {
            audioSource.volume += FinalVolume * Time.deltaTime / FadeTime;

            yield return null;
        }


        audioSource.volume = FinalVolume;

    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        
    }



    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {

            Debug.LogWarning("Sound : " + name + " not found !");
            return;

        }

        s.source.volume = s.volume * AudioManager.SoundEffectsVolume;

        if (!s.alreadyPlayed)
        {
            s.source.Play();


        }

    }
}
