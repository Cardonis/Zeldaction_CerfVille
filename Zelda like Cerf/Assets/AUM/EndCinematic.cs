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

        animatorPlayer.transform.position = mainPlayer.transform.position;

        gameObject.SetActive(false);
    }
}
