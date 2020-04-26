using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public Music[] Musics;

    public Music[] Ambiances;

    private AudioSource[] Audiosources;

    public static MusicManager instance;

    public Music CurrentMusic;

    public Music CurrentAmbiance;

    public bool InBattle;

    public enum PlayerPos { Village, Graveyard, Donjon01, Donjon02, Forest01, Forest02, Boss};

    public PlayerPos PlayerCurrentPos;
    void Awake()
    {

        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        
        foreach (Music s in Musics)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume;

            s.source.loop = s.loop;

        }

        foreach (Music s in Ambiances)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume;

            s.source.loop = s.loop;

        }


    }

    // Update is called once per frame
    void Update()
    {
        CurrentMusic = Array.Find(Musics, s => s.name == "Village");

        if (CurrentMusic == null)
        {






        }







    }

    public IEnumerator FadeOut (AudioSource audioSource, float FadeTime)
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

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float FinalVolume)
    {
        float FinishVolume = FinalVolume;
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < FinishVolume)
        {
            audioSource.volume += FinishVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        
        audioSource.volume = FinishVolume;

    }

    void PlayMusic()
    {

        switch (PlayerCurrentPos)
        {
            case PlayerPos.Village:
                CurrentMusic = Array.Find(Musics, s => s.name == "Village");
                FadeIn(CurrentMusic.source, 3, 1);
                break;
            case PlayerPos.Graveyard:
                CurrentMusic = Array.Find(Musics, s => s.name == "Graveyard");
                FadeIn(CurrentMusic.source, 3, 1);
                break;
            case PlayerPos.Donjon01:
                CurrentMusic = Array.Find(Musics, s => s.name == "Graveyard");
                FadeIn(CurrentMusic.source, 3, 1);
                break;
            case PlayerPos.Donjon02:
                break;
            case PlayerPos.Forest01:
                break;
            case PlayerPos.Forest02:
                break;
            case PlayerPos.Boss:
                break;
            default:
                break;
        }






    }


}
