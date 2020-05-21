﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCinematic : MonoBehaviour
{
    public Transform mainPlayer;
    public Animator animatorPlayer;

    private void OnEnable()
    {
        StartCoroutine (ResetGraphPosition());
    }

    IEnumerator ResetGraphPosition()
    {


        yield return new WaitForSeconds(0.05f);

        mainPlayer.transform.position = animatorPlayer.transform.position;

        animatorPlayer.transform.position = mainPlayer.transform.position;

        gameObject.SetActive(false);
    }
}
