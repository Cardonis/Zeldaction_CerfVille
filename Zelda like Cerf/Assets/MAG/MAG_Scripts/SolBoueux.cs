using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolBoueux : MonoBehaviour
{
    
    private Player_Main_Controller player;
    private Caisse_Controller pierre;
    [HideInInspector]
    public bool isTractable;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller playerTest = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if(playerTest != null)
        {
            player = playerTest;
            Debug.Log("1 " + player);
            player.canTrack = false;
        }

        pierre = collider.transform.parent.GetComponent<Caisse_Controller>();

        if (pierre != null)
        {
            pierre.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        Debug.Log("2 " + player);
        if (pierre != null && player!= null)
        {
            if (player.currentPierre != null)
            {
            player.canTrack = false;
            Physics2D.IgnoreCollision(player.physicCollider, player.currentPierre.GetComponentInChildren<Collider2D>(), false);
            player.currentPierre = null;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Player_Main_Controller playerTest = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();

        pierre = collider.transform.parent.GetComponent<Caisse_Controller>();

        if (playerTest != null)
        {
            playerTest.canTrack = true;
        }
        if (pierre != null)
        {
            pierre.rb.constraints = RigidbodyConstraints2D.None;
            pierre.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


}
