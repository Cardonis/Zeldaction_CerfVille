using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;
    public bool IsPlaying;

    [HideInInspector]
    public AudioSource source;



}
