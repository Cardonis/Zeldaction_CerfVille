using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonmanager : MonoBehaviour
{
    [HideInInspector] public bool isPressed;

    public Renderer dissolveMaterial;

    [HideInInspector] public Coroutine lastLineLit;

    [HideInInspector] public Coroutine lastStarting;

    public IEnumerator StartLineLit(float valueFrom, float valueTo, float timeFor)
    {
        while(lastLineLit != null)
        {
            yield return null;
        }

        lastLineLit = StartCoroutine(LineLitFromToFor(valueFrom, valueTo, timeFor));

        lastStarting = null;
    }

    public IEnumerator LineLitFromToFor(float valueFrom, float valueTo, float timeFor)
    {
        float elapsedTime = 0.0f;

        dissolveMaterial.material.SetFloat("_Fade", valueTo);

        while (elapsedTime < timeFor)
        {
            elapsedTime += Time.deltaTime;
            dissolveMaterial.material.SetFloat("_Fade", valueFrom - Mathf.Clamp01(elapsedTime / timeFor) * (valueFrom - valueTo) );
            yield return null;
        }

        dissolveMaterial.material.SetFloat("_Fade", valueTo);

        lastLineLit = null;
    }
}
