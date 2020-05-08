using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Player_Main_Controller player;

    private void Start()
    {
        Debug.Log("oui");
        player.StartCoroutine(player.StunnedFor(17));
    }
    public void Play()
    {
        playableDirector.Play();
    }
}
