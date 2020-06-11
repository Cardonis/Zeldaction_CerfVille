using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XOnStone : MonoBehaviour
{
    Player_Main_Controller truePlayer;
    Caisse_Controller pierre;

    private void Start()
    {
        pierre = gameObject.GetComponent<Caisse_Controller>();
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.attachedRigidbody != null)
        {
            Player_Main_Controller player = collider.attachedRigidbody.GetComponent<Player_Main_Controller>();
            if (player != null && pierre.isTractable == true)
            {
                truePlayer = player;
                truePlayer.pressX.SetActive(true);
            }
        }

    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.attachedRigidbody != null)
        {
            Player_Main_Controller player = collider.attachedRigidbody.GetComponent<Player_Main_Controller>();
            if (player != null)
            {
                truePlayer.pressX.SetActive(false);
            }
        }
    }
}
