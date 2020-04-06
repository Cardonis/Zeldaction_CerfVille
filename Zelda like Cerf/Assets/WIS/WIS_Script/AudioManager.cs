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


    // Update is called once per frame
    void Update()
    {

        if (player.baseAttacking == true)
        {

            Play("Coup_de_vent");


        }

        if (player.life != playerlife)
        {

            Play("Player_take_damage");

        }

        if (Input.GetButton("X"))
        {
            if (buttonXcharge > 0 && buttonXcharge < 1)
            {

                Play("Capa_charge_niv1");
                AlreadyPlay("Capa_charge_niv1");
                buttonXcharge += Time.deltaTime;
                nivCharge = 1;

            } else if (buttonXcharge > 1 && buttonXcharge < 2)
            {

                Play("Capa_charge_niv2");
                AlreadyPlay("Capa_charge_niv2");
                buttonXcharge += Time.deltaTime;
                nivCharge = 2;

            } else if (buttonXcharge > 2 && buttonXcharge < 3)
            {

                Play("Capa_charge_niv3");
                AlreadyPlay("Capa_charge_niv3");
                buttonXcharge += Time.deltaTime;
                nivCharge = 3;

            } else
            {

                buttonXcharge += Time.deltaTime;

            }

        }

        if(Input.GetButtonUp("X"))
        {

            RePlay("Capa_charge_niv1");
            RePlay("Capa_charge_niv2");
            RePlay("Capa_charge_niv3");

            switch (nivCharge)
            {

                case 1:
                    Play("Capa_simple_niv1");
                    nivCharge = 0;
                    break;

                case 2:
                    Play("Capa_simple_niv2");
                    nivCharge = 0;
                    break;

                case 3:
                    Play("Capa_simple_niv3");
                    nivCharge = 0;
                    break;

                default:
                    break;
            }


        }
        
        playerlife = player.life;

    }
}
