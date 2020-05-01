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

    public static bool InBattle;

    public enum PlayerPos { Village, Graveyard, Donjon01, Forest01, Forest02, Boss };

    public static PlayerPos PlayerCurrentPos;
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
        if (!InBattle)
        {
            PlayMusic();
        }
        else
        {
            PlayBattleMusic();
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
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < FinalVolume)
        {
            audioSource.volume += FinalVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        
        audioSource.volume = FinalVolume;

    }

    void PlayMusic()
    {

        switch (PlayerCurrentPos)
        {
            case PlayerPos.Village:
                if (CurrentMusic.name == "Village") {break;}
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Village");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
                break;
            case PlayerPos.Graveyard:
                if (CurrentMusic.name == "Graveyard") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Graveyard");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
                break;
            case PlayerPos.Donjon01:
                if (CurrentMusic.name == "Donjon01") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Donjon01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
                break;
            case PlayerPos.Forest01:
                if (CurrentMusic.name == "Forest01") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
                break;
            case PlayerPos.Forest02:
                if (CurrentMusic.name == "Forest02") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest02");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
                break;
            case PlayerPos.Boss:
                if (CurrentMusic.name == "Boss") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Boss");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
                break;
            default:
                break;
        }






    }

    void PlayBattleMusic()
    {
        if (PlayerCurrentPos == PlayerPos.Village || PlayerCurrentPos == PlayerPos.Graveyard || PlayerCurrentPos == PlayerPos.Forest01 || PlayerCurrentPos == PlayerPos.Forest02)
        {
            if (CurrentMusic.name != "Battle01")
            {
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Battle01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
            }
        }
        else if (PlayerCurrentPos == PlayerPos.Donjon01)
        {
            if (CurrentMusic.name != "Battle02")
            {
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Battle02");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
            }
        }
        else if (PlayerCurrentPos == PlayerPos.Boss)
        {
            if (CurrentMusic.name != "Boss")
            {
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Boss");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, 1));
            }
        }




    }
}
