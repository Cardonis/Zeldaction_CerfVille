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
      
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if( player != null)
        {
            player.canTrack = false;
        }

        pierre = collider.transform.parent.GetComponent<Caisse_Controller>();

        if (pierre != null)
        {
            pierre.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();

        pierre = collider.transform.parent.GetComponent<Caisse_Controller>();

        if (player != null)
        {
            player.canTrack = true;
        }
        if (pierre != null)
        {
            pierre.rb.constraints = RigidbodyConstraints2D.None;
            pierre.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


}
