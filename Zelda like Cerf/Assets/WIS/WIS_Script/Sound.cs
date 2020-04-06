﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;
    public bool alreadyPlayed;

    [HideInInspector]
    public AudioSource source;



}
