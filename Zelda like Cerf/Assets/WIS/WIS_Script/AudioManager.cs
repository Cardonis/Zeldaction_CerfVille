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


    
    
    void Awake()
    {

        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }



        DontDestroyOnLoad(gameObject);
        //On instancie les différents audiospurces pour chaque sons
        foreach (Sound s in sounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;

            s.source.volume = s.volume;

            s.source.loop = s.loop;

        }



    }

    private void Start()
    {

    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {

            Debug.LogWarning("Sound : " + name + " not found !");
            return;

        }

        

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

        s.source.volume = s.volume;

        s.source.loop = s.loop;

        s.source.Play();


    }

    public void StopPlay()
    {
        Destroy(GetComponent<AudioSource>());
    }

    public IEnumerator PlayOne(string name, GameObject here)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source = here.AddComponent<AudioSource>();
        s.source.clip = s.Clip;

        s.source.volume = s.volume;

        s.source.loop = s.loop;

        s.source.Play();

        yield return new WaitForSecondsRealtime(s.source.clip.length);

        s.source.Stop();
        Destroy(s.source);
    }

    private void WaitForSeconds()
    {
        throw new NotImplementedException();
    }

    public void RePlay (string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {

            Debug.LogWarning("Sound : " + name + " not found !");
            return;

        }

        s.alreadyPlayed = false;
                              
    }

    public void AlreadyPlay(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {

            Debug.LogWarning("Sound : " + name + " not found !");
            return;

        }

        s.alreadyPlayed = true;

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


    // Update is called once per frame
    void Update()
    {





    }
}
