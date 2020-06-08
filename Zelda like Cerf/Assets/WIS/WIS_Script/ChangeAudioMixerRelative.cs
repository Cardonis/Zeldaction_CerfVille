using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAudioMixerRelative : MonoBehaviour
{
    private AudioSource[] audioSources;




    // Start is called before the first frame update
    void Start()
    {
        audioSources = gameObject.GetComponents<AudioSource>();




    }

    // Update is called once per frame
    void Update()
    {

        foreach (AudioSource s in audioSources)
        {

            s.volume *= AudioManager.SoundEffectsVolume;

        }


    }
}
