using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AllTimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    bool alreadyPlayed;
    public int playerCantMoveFor;
    public bool setPlayerTransform;

    private void Start()
    {
        alreadyPlayed = false;
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller player;
        player = collider.GetComponentInParent<Player_Main_Controller>();
        if (setPlayerTransform == true && alreadyPlayed == false)
        {
            player.transform.position = new Vector3(-0.05f, 30.51f, 0);
        }
        if (player != null && collider.gameObject.name == "PPhysicsCollider" && alreadyPlayed == false)
        {
            player.StartCoroutine(player.StunnedFor(playerCantMoveFor));
            playableDirector.Play();
            alreadyPlayed = true;
        }
    }
}
