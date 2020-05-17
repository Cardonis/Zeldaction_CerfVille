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
    public bool InBattleP;
    public static int EnemyInBattle;
    public int EnemyInBattleP;

    [Range(0f, 1f)]
    public static float MusicVolume = 1;

    private float VolumeChange;

    public enum PlayerPos { Village, Graveyard, Donjon01, Forest01, Forest02, Boss };

    
    public static PlayerPos PlayerCurrentPos;

    public PlayerPos DefaultPosition;

    private bool IsChangingMusic;
    void Awake()
    {

        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        foreach (Music s in Musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume * MusicVolume;

            s.source.loop = s.loop;
        }

        foreach (Music s in Ambiances)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume * MusicVolume;

            s.source.loop = s.loop;
        }

        VolumeChange = MusicVolume;

        PlayerCurrentPos = DefaultPosition;

        switch (DefaultPosition)
        {
            case PlayerPos.Village:
                CurrentMusic = Array.Find(Musics, s => s.name == "Village");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Graveyard:
                CurrentMusic = Array.Find(Musics, s => s.name == "Graveyard");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Donjon01:
                CurrentMusic = Array.Find(Musics, s => s.name == "Donjon01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Forest01:
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Forest02:
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest02");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Boss:
                CurrentMusic = Array.Find(Musics, s => s.name == "Boss");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            default:
                break;
        }

        IsChangingMusic = false;
    }

    // Update is called once per frame
    void Update()
    {
        InBattleP = InBattle;
        EnemyInBattleP = EnemyInBattle;

        if (MusicVolume != VolumeChange)
        {
            foreach (Music s in Musics)
            {
                s.source.volume = MusicVolume;
            }


            VolumeChange = MusicVolume;

        }
        if (IsChangingMusic == false)
        {

            if (!InBattle)
            {
                PlayMusic();
            }
            else
            {
                PlayBattleMusic();
            }

            if (MusicManager.EnemyInBattle <= 0)
            {
                MusicManager.EnemyInBattle = 0;
                MusicManager.InBattle = false;
            }

        }

    }

    public IEnumerator FadeOut (AudioSource audioSource, float FadeTime)
    {
        IsChangingMusic = true;
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        IsChangingMusic = false;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float FinalVolume)
    {
        IsChangingMusic = true;
        audioSource.volume = 0;
        audioSource.Play();

        FinalVolume = FinalVolume * MusicVolume;

        while (audioSource.volume < FinalVolume)
        {
            audioSource.volume += FinalVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        
        audioSource.volume = FinalVolume;
        IsChangingMusic = false;
    }

    void PlayMusic()
    {

        switch (PlayerCurrentPos)
        {
            case PlayerPos.Village:
                if (CurrentMusic.name == "Village") {break;}
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Village");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Graveyard:
                if (CurrentMusic.name == "Graveyard") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Graveyard");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Donjon01:
                if (CurrentMusic.name == "Donjon01") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Donjon01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Forest01:
                if (CurrentMusic.name == "Forest01") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Forest02:
                if (CurrentMusic.name == "Forest02") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest02");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            case PlayerPos.Boss:
                if (CurrentMusic.name == "Boss") { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Boss");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                break;
            default:
                break;
        }






    }

    void PlayBattleMusic()
    {
            if (CurrentMusic.name != "Battle01" && CurrentMusic.name != "Battle02" && EnemyInBattle > 0 && EnemyInBattle < 4)
            {
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Battle01");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
            }
    
            if (CurrentMusic.name != "Battle02" && EnemyInBattle > 3)
            {
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Battle02");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
            }
        
            if (CurrentMusic.name != "Boss" && PlayerCurrentPos == PlayerPos.Boss)
            {
                StartCoroutine(FadeOut(CurrentMusic.source, 3));
                CurrentMusic = Array.Find(Musics, s => s.name == "Boss");
                StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
            }
    }




    public void PlayAmbiance()
    {


    }



}
