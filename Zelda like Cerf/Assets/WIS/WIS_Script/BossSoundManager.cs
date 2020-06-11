using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class BossSoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public Music[] Musics;

    public BossBehavior bossBehavior;

    public Music CurrentMusic;

    public int phaseBoss;
    public bool IsInBattle;
    public bool IsEnd;

    public GameObject InBattle;
    public GameObject End;

    // Start is called before the first frame update
    void Start()
    {
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

        IsEnd = false;
        IsInBattle = false;
    }

    // Update is called once per frame
    void Update()
    {

        phaseBoss = bossBehavior.phase;

        if (!InBattle.activeSelf)
        {

            IsInBattle = true;

        } else { IsInBattle = false; }

        if (!End.activeSelf)
        {

            IsEnd = true;
            StopMusic();

        }

        if(IsEnd == false)
        {

            PlayMusic();
            

        }



        if (bossBehavior.pv == 0 && IsInBattle == true)
        {
            InBattle.SetActive(true);

        }



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


            s.source.Play();
    }

    public void PlayMusic()
    {
        if (IsInBattle == true)
        {
            switch (phaseBoss)
            {
                case 1:
                    if (CurrentMusic.name == "Boss_Part1") { break; }
                    StartCoroutine(FadeOut(CurrentMusic.source, 1));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Boss_Part1");
                    StartCoroutine(FadeIn(CurrentMusic.source, 1, CurrentMusic.volume));
                    break;
                case 2:
                    if (CurrentMusic.name == "Boss_Part1") { break; }
                    StartCoroutine(FadeOut(CurrentMusic.source, 1));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Boss_Part1");
                    StartCoroutine(FadeIn(CurrentMusic.source, 1, CurrentMusic.volume));
                    break;
                case 3:
                    if (CurrentMusic.name == "Boss_Part2") { break; }
                    StartCoroutine(FadeOut(CurrentMusic.source, 1));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Boss_Part2");
                    StartCoroutine(FadeIn(CurrentMusic.source, 1, CurrentMusic.volume));
                    break;
                case 4:
                    if (CurrentMusic.name == "Boss_Part2") { break; }
                    StartCoroutine(FadeOut(CurrentMusic.source, 1));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Boss_Part2");
                    StartCoroutine(FadeIn(CurrentMusic.source, 1, CurrentMusic.volume));
                    break;
                default:
                    break;
            }
        }
        else if (CurrentMusic.name != "Boss_Intro" && phaseBoss <= 1 && MusicManager.PlayerCurrentPos == MusicManager.PlayerPos.Boss)
        {
            StartCoroutine(FadeOut(CurrentMusic.source, 1));
            CurrentMusic = Array.Find(Musics, s => s.name == "Boss_Intro");
            StartCoroutine(FadeIn(CurrentMusic.source, 1, CurrentMusic.volume));
        }
        else
        {
            switch (phaseBoss)
            {
                case 4:
                    if (CurrentMusic.name == "Boss_Fin") { break; }
                    StartCoroutine(FadeOut(CurrentMusic.source, 1));
                    CurrentMusic = Array.Find(Musics, s => s.name == "Boss_Fin");
                    StartCoroutine(FadeIn(CurrentMusic.source, 1, CurrentMusic.volume));
                    break;
                default:
                    break;
            }

        }






    }

    public void StopMusic()
    {
        if (CurrentMusic != null)
        {

            StartCoroutine(FadeOut(CurrentMusic.source, 1));
            CurrentMusic = null;
        }

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

}
