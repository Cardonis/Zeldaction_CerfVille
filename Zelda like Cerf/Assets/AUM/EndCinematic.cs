using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCinematic : MonoBehaviour
{
    public Transform mainPlayer;
    public Animator animatorPlayer;

    private void OnEnable()
    {
        ResetGraphPosition();
    }

    void ResetGraphPosition()
    {

        mainPlayer.transform.position = animatorPlayer.transform.position;

        animatorPlayer.enabled = false;

        animatorPlayer.transform.position = mainPlayer.transform.position;

        animatorPlayer.enabled = true;

        gameObject.SetActive(false);
    }
}
