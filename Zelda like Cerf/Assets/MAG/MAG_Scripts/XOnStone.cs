using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XOnStone : MonoBehaviour
{
    Player_Main_Controller truePlayer;
    public void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();

        if (player != null)
        {
            truePlayer = player;
            truePlayer.pressX.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        Player_Main_Controller player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();

        if(player != null)
        {
            truePlayer.pressX.SetActive(false);
        }
        
    }
}
