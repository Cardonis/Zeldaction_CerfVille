using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AllTimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    bool alreadyPlayed;
    public bool wasPlayed = false;
    public int playerCantMoveFor;
    public bool setPlayerTransform;

    public bool cinematicIsInteraction;
    bool playerIsNear;

    Player_Main_Controller player;

    private void Start()
    {
        alreadyPlayed = false;
        playerIsNear = false;
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        player = collider.GetComponentInParent<Player_Main_Controller>();
        if (setPlayerTransform == true && alreadyPlayed == false)
        {
            player.transform.position = new Vector3(-0.05f, 30.51f, 0);
        }
        if (player != null && collider.gameObject.name == "PPhysicsCollider" && alreadyPlayed == false )
        {
            if (cinematicIsInteraction == false)
            {
                LaunchTimeline(player);
            }
            else
            {
                    playerIsNear = true;
                    player.pressX.SetActive(true);
                
            }
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player != null && cinematicIsInteraction == true)
            player.pressX.SetActive(false);
        playerIsNear = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("X") && playerIsNear == true && alreadyPlayed== false)
        {
            player.pressX.SetActive(false);
            LaunchTimeline(player);
        }

        if(wasPlayed == true)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void LaunchTimeline(Player_Main_Controller player)
    {
        player.StartCoroutine(player.StunnedFor(playerCantMoveFor));
        StartCoroutine(WasPlayedAfter(playerCantMoveFor));
        playableDirector.Play();
        alreadyPlayed = true;
    } 

    IEnumerator WasPlayedAfter(float time)
    {
        yield return new WaitForSeconds(time);

        wasPlayed = true;
    }
}
