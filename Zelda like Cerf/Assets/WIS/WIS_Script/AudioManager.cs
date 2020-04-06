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

    public static AudioManager instance;

    private Player_Main_Controller player;

    //Liste des variables pour les différentes valeurs que l'audiomanager doit garder a jour pour savoir
    //quand jouer les sons

    private int playerlife; //Les points de vie actuels du joueur
    private float buttonXcharge; //Le temps que le joueur reste appuyé sur le bouton

    private int nivCharge;
    
    
    void Awake()
    {

        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();

        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }



        DontDestroyOnLoad(gameObject);

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

        playerlife = player.life;

        buttonXcharge = 0;




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
