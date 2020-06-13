using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public GameObject PlayingMusic;
    public GameObject EncounterMusic;
    public GameObject PlayForest02;

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

    public enum PlayerPos { Village, Graveyard, Donjon01, Forest01, Forest02, Boss, Encounter };

    
    public static PlayerPos PlayerCurrentPos;


    public PlayerPos DefaultPosition;

    public bool IsChangingMusic;
    private bool MusicPause;
    public static bool Encounter;

    public AudioMixer audioMaster;

    public PlayerPos playerCurrentPosP;

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

        VolumeChange = 0;

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
        MusicPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        //CinematicVolume();

        InBattleP = InBattle;
        EnemyInBattleP = EnemyInBattle;

        playerCurrentPosP = PlayerCurrentPos;

        if (MusicVolume != VolumeChange)
        {
            foreach (Music s in Musics)
            {
                s.source.volume = s.volume * MusicVolume;
            }


            VolumeChange = MusicVolume;

        }
        if (IsChangingMusic == false)
        {

            if (!InBattle && EnemyInBattle <= 0)
            {
                PlayMusic();
            }
            else if (InBattle && EnemyInBattle > 0)
            {
                PlayBattleMusic();
            }

            if (MusicManager.EnemyInBattle <= 0)
            {
                MusicManager.EnemyInBattle = 0;
                MusicManager.InBattle = false;
            }

        }

        if (PlayingMusic.activeSelf == false && MusicPause == false && IsChangingMusic == false)
        {
            StartCoroutine(PausePlayingMusic());

            MusicPause = true;
        }

        if (EncounterMusic.activeSelf == false && MusicPause == false && IsChangingMusic == false)
        {

            PlayerCurrentPos = PlayerPos.Encounter;
            Encounter = true;

        }

        if (PlayForest02.activeSelf == false && MusicPause == false && IsChangingMusic == false)
        {

            PlayerCurrentPos = PlayerPos.Forest02;
            Encounter = false;

        }

        if (!InBattle && EnemyInBattle <= 0 && IsChangingMusic == false && CurrentMusic.name != "Battle01" && CurrentMusic.name != "Battle02")
        {

            PlayMusic();


        }


    }

    public IEnumerator FadeOut (AudioSource audioSource, float FadeTime)
    {
        //IsChangingMusic = true;
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        //IsChangingMusic = false;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float FinalVolume)
    {
        //IsChangingMusic = true;
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
                IsChangingMusic = true;
                StartCoroutine(FadeOut(CurrentMusic.source, 2));
                CurrentMusic = Array.Find(Musics, s => s.name == "Village");
                StartCoroutine(FadeIn(CurrentMusic.source, 2, CurrentMusic.volume));                
                break;
            case PlayerPos.Graveyard:
                if (CurrentMusic.name == "Graveyard") { break; }
                IsChangingMusic = true;
                StartCoroutine(FadeOut(CurrentMusic.source, 2));
                CurrentMusic = Array.Find(Musics, s => s.name == "Graveyard");
                StartCoroutine(FadeIn(CurrentMusic.source, 2, CurrentMusic.volume));
                break;
            case PlayerPos.Donjon01:
                if (CurrentMusic.name == "Donjon01") { break; }
                IsChangingMusic = true;
                StartCoroutine(FadeOut(CurrentMusic.source, 2));
                CurrentMusic = Array.Find(Musics, s => s.name == "Donjon01");
                StartCoroutine(FadeIn(CurrentMusic.source, 2, CurrentMusic.volume));
                break;
            case PlayerPos.Forest01:
                if (CurrentMusic.name == "Forest01") { break; }
                IsChangingMusic = true;
                StartCoroutine(FadeOut(CurrentMusic.source, 2));
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest01");
                StartCoroutine(FadeIn(CurrentMusic.source, 2, CurrentMusic.volume));
                break;
            case PlayerPos.Forest02:
                if (CurrentMusic.name == "Forest02") { break; }
                IsChangingMusic = true;
                StartCoroutine(FadeOut(CurrentMusic.source, 2));
                CurrentMusic = Array.Find(Musics, s => s.name == "Forest02");
                StartCoroutine(FadeIn(CurrentMusic.source, 2, CurrentMusic.volume));
                break;
            case PlayerPos.Boss:
                if (CurrentMusic == null) { break; }
                StartCoroutine(FadeOut(CurrentMusic.source, 2));
                CurrentMusic = null;
                break;
            case PlayerPos.Encounter:
                if (CurrentMusic.name == "Encounter") { break; }
                IsChangingMusic = true;
                StartCoroutine(FadeOut(CurrentMusic.source, 2));
                CurrentMusic = Array.Find(Musics, s => s.name == "Encounter");
                StartCoroutine(FadeIn(CurrentMusic.source, 2, CurrentMusic.volume));
                break;
            default:
                break;
        }






    }

    void PlayBattleMusic()
    {
            if (PlayerCurrentPos != PlayerPos.Encounter)
            {

                if (CurrentMusic.name != "Battle01" && CurrentMusic.name != "Battle02" && EnemyInBattle > 0 && EnemyInBattle < 4)
                {
                    IsChangingMusic = true;
                    StartCoroutine(FadeOut(CurrentMusic.source, 3));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Battle01");
                    StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                }
    
                if (CurrentMusic.name != "Battle02" && EnemyInBattle > 3)
                {
                    IsChangingMusic = true;
                    StartCoroutine(FadeOut(CurrentMusic.source, 3));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Battle02");
                    StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                }
        
                if (CurrentMusic.name != "Boss" && PlayerCurrentPos == PlayerPos.Boss)
                {
                    IsChangingMusic = true;
                    StartCoroutine(FadeOut(CurrentMusic.source, 3));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Boss");
                    StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
                }

            }
            else if (PlayerCurrentPos == PlayerPos.Encounter && EnemyInBattle > 0 && CurrentMusic.name != "Encounter_Battle")
            {
            IsChangingMusic = true;
            StartCoroutine(FadeOut(CurrentMusic.source, 3));
            CurrentMusic = Array.Find(Musics, s => s.name == "Encounter_Battle");
            StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));
            }
    }

    public IEnumerator PausePlayingMusic()
    {

        MusicPause = true;

        StartCoroutine(FadeOut(CurrentMusic.source, 3));

        while (PlayingMusic.activeSelf == false)
        {


            yield return null;
        }

        StartCoroutine(FadeIn(CurrentMusic.source, 3, CurrentMusic.volume));

        MusicPause = false;
    }


    public void PlayAmbiance()
    {


    }

    public void CinematicVolume()
    {

        audioMaster.SetFloat("CinematicMusic", Mathf.Log10(MusicVolume) * 20);

    }


}
