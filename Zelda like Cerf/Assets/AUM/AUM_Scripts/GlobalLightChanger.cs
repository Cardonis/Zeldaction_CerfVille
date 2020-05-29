using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightChanger : MonoBehaviour
{
    public Light2D globalLight2D;

    public Color color;
    [Range(0f, 1f)] public float intensity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody.tag == "Player")
        StartCoroutine(ChangeValues(1f));
    }

    public IEnumerator ChangeValues(float time)
    {
        Color c = globalLight2D.color;

        float baseIntensity = globalLight2D.intensity;
        float baseR = c.r;
        float baseG = c.g;
        float baseB = c.b;

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight2D.intensity = baseIntensity + (intensity - baseIntensity) * (i / time);

            c.r = baseR + (color.r - baseR) * (i / time);
            c.g = baseG + (color.g - baseG) * (i / time);
            c.b = baseB + (color.b - baseB) * (i / time);

            globalLight2D.color = c;

            yield return null;
        }

        globalLight2D.color = color;

        globalLight2D.intensity = intensity;
    }
}
