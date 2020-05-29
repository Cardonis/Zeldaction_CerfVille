﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TelegraphAttack : MonoBehaviour
{
    public Light2D lightt;

    public float baseIntensity;

    private void Start()
    {
        baseIntensity = lightt.intensity;
        lightt.intensity = 0;
    }

    public IEnumerator FlashLight(float speed)
    {
        while(lightt.intensity <= baseIntensity)
        {
            lightt.intensity = lightt.intensity + speed * Time.deltaTime;
            yield return null;
        }

        lightt.intensity = baseIntensity;

        while (lightt.intensity >= 0)
        {
            lightt.intensity = lightt.intensity - speed * Time.deltaTime;
            yield return null;
        }

        lightt.intensity = 0;
    }

    public IEnumerator LitLight(float speed, float intensityToGo)
    {
        while (lightt.intensity <= intensityToGo)
        {
            lightt.intensity = lightt.intensity + speed * Time.deltaTime;
            yield return null;
        }

        lightt.intensity = intensityToGo;
    }

    public IEnumerator UnlitLight(float speed)
    {
        while (lightt.intensity >= 0)
        {
            lightt.intensity = lightt.intensity - speed * Time.deltaTime;
            yield return null;
        }

        lightt.intensity = 0;
    }
}
