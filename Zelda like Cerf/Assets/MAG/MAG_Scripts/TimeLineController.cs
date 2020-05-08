using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Player_Main_Controller player;

    private void Start()
    {
        player.StartCoroutine(player.StunnedFor(17));
    }
    public void Play()
    {
        playableDirector.Play();
    }
}
