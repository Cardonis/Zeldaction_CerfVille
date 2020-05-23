using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonmanager : MonoBehaviour
{
    [HideInInspector] public bool isPressed;

    public Renderer dissolveMaterial;

    /*IEnumerator LineLitFromToFor(float valueFrom, float valueTo, float timeFor)
    {
        float elapsedTime = 0.0f;

        Color c = dissolveMaterial.color;

        while (elapsedTime < timeFor)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / (timeFor));
            player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = c;
            yield return null;
        }

        c.a = 1.0f;
        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = c;

        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].gameObject.SetActive(false);
    }*/
}
