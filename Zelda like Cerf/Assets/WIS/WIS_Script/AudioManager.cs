using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // C'est la script qui gère la liste de tous les son et qui permet de les jouer quand on veux
    // les jouer !

    // Liste des variables nésséssaire pour jouer les son et les stocker.
    public Sound[] sounds;

    private AudioSource[] Audiosources;

    public static AudioManager instance;

    private Player_Main_Controller player;

    [Range(0f, 1f)]
    public static float SoundEffectsVolume = 1;

    private float VolumeChange;

    private InventoryBook inventoryBook;

    Vector2 input;

    private bool DeplacementPlaying;
    private bool ButtonPlaying;

    public AudioMixer audioMaster;
    void Awake()
    {

        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }




        //On instancie les différents audiospurces pour chaque sons
        foreach (Sound s in sounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume * SoundEffectsVolume;

            s.source.loop = s.loop;

        }



    }

    private void Start()
    {
        inventoryBook = GameObject.Find("InventoryBook").GetComponent<InventoryBook>();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input.magnitude > 0 && inventoryBook.iventoryIsOpen == false)
        {
            if (DeplacementPlaying == false && MusicManager.PlayerCurrentPos != MusicManager.PlayerPos.Donjon01) { StartCoroutine(PlayOneOf("D1", "D2", "D3")); }
            else if (DeplacementPlaying == false) { StartCoroutine(PlayOneOf("D4", "D5", "D6")); }
        }


        CinematicVolume();

            


        

    }

    public void CinematicVolume ()
    {
        audioMaster.SetFloat("CinematicVolume", Mathf.Log10(SoundEffectsVolume) * 20);
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {

            Debug.LogWarning("Sound : " + name + " not found !");
            return;

        }

        s.source.volume = s.volume * SoundEffectsVolume;

        if (!s.alreadyPlayed)
        {
            s.source.Play();
            

        }

    }

    public void PlayHere(string name, GameObject here)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {

            Debug.LogWarning("Sound : " + name + " not found !");
            return;

        }

        s.source = here.AddComponent<AudioSource>();
        s.source.clip = s.Clip;

        s.source.volume = s.volume * SoundEffectsVolume;

        s.source.loop = s.loop;

        s.source.Play();


    }

    public IEnumerator PlayOne(string name, GameObject here)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        
        AudioSource thisSource = here.AddComponent<AudioSource>();

        thisSource.clip = s.Clip;

        thisSource.volume = s.volume * SoundEffectsVolume;

        thisSource.loop = s.loop;

        thisSource.Play();

        yield return new WaitForSecondsRealtime(s.source.clip.length);

        thisSource.Stop();

        Destroy(thisSource);

    }

    public IEnumerator PlayAndFadeOut(string name, GameObject here, float FadeTime)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source = here.AddComponent<AudioSource>();
        s.source.clip = s.Clip;

        s.source.volume = s.volume * SoundEffectsVolume;

        s.source.loop = s.loop;

        s.source.Play();

        float startVolume = s.source.volume;
        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.source.Stop();


        Destroy(s.source);


    }



    public IEnumerator PlayOneOf(string name1, string name2, string name3)
    {

            int temp = UnityEngine.Random.Range(0,2);

            string musicName = "";

            switch (temp)
            {
                case 0:
                    musicName = name1;
                    break;
                case 1:
                    musicName = name2;
                    break;
                default:
                    break;
            }

            Sound s = Array.Find(sounds, sound => sound.name == musicName);


            AudioSource thisSource = s.source;

            thisSource.clip = s.Clip;

            thisSource.volume = s.volume * SoundEffectsVolume;

            thisSource.loop = s.loop;

            DeplacementPlaying = true;

            thisSource.Play();            

            yield return new WaitForSecondsRealtime(s.source.clip.length);

            thisSource.Stop();

            //yield return new WaitForSecondsRealtime(0.1f);

            DeplacementPlaying = false;
    }


    private void WaitForSeconds()
    {
        throw new NotImplementedException();
    }




    public void Stop(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {

            Debug.LogWarning("Sound : " + name + " not found !");
            return;

        }

        s.source.Stop();

    }


   

}
