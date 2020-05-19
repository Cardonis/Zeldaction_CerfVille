using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolBoueux : MonoBehaviour
{
    
    private Player_Main_Controller player;
    private List<Caisse_Controller> pierres;
    private Caisse_Controller pierre;
    [HideInInspector]
    public bool isTractable;
    public float boueDrag;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
        pierres = new List<Caisse_Controller>();
    }

    private void Update()
    {
            foreach (Caisse_Controller pierre1 in pierres)
            {
                if (Mathf.Approximately(0, pierre1.rb.velocity.magnitude))
                {
                    pierre1.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    pierre1.rb.drag = boueDrag;
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
            pierres.Add(pierre);
            pierre.isTractable = false;
            if (pierre == player.currentPierre)
            {
                Physics2D.IgnoreCollision(player.physicCollider, player.currentPierre.GetComponentInChildren<Collider2D>(), false);
                player.currentPierre = null;
            }
            pierre.rb.constraints = RigidbodyConstraints2D.FreezeAll;
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
            if (pierres.Contains(pierre))
            {

                pierre.isTractable = true;
                pierre.rb.constraints = RigidbodyConstraints2D.None;
                pierre.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                pierre.rb.drag = pierre.initialDrag;
                pierres.Remove(pierre);
            }
        }

    }


}
