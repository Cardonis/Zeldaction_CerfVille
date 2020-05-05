using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolBoueux : MonoBehaviour
{
    
    private Player_Main_Controller player;
    private Caisse_Controller pierre;
    [HideInInspector]
    public bool isTractable;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }

    private void Update()
    {
        if(pierre != null)
        {
            if(Mathf.Approximately(0, pierre.rb.velocity.magnitude))
            {
                pierre.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponentInParent<Player_Main_Controller>() == player)
        {
            player.canTrack = false;
        }
        pierre = collider.transform.parent.GetComponent<Caisse_Controller>();

        if (pierre != null)
        {
            pierre.isTractable = false;
            if (pierre == player.currentPierre)
            {
                Physics2D.IgnoreCollision(player.physicCollider, player.currentPierre.GetComponentInChildren<Collider2D>(), false);
                player.currentPierre = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        pierre = collider.transform.parent.GetComponent<Caisse_Controller>();


        if (collider.GetComponentInParent<Player_Main_Controller>() == player)
        {
            player.canTrack = true;
        }

        if (pierre != null)
        {
            pierre.isTractable = true;
            pierre.rb.constraints = RigidbodyConstraints2D.None;
            pierre.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            pierre = null;
        }
    }


}
