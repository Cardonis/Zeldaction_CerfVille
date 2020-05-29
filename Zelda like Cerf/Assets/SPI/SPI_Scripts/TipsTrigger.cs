using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsTrigger : MonoBehaviour
{

    Player_Main_Controller player;

    bool playerIsNear=false;
    public bool asTalked;


    public TipsPlayer tipsPlayer;

    public OutlineController outlineController;

    public GameObject theTips;

    private void Update()
    {

        if (Input.GetButtonDown("X") && tipsPlayer.isInTips == false && playerIsNear == true && asTalked == false)
        {
            asTalked = true;
            player.pressX.SetActive(false);
            StartCoroutine(tipsPlayer.OpenTips(theTips));

            outlineController.outLining = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller p = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (p != null)
        {
            player = p;
            playerIsNear = true;
            player.pressX.SetActive(true);

            outlineController.outLining = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Player_Main_Controller player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            player.pressX.SetActive(false);
            playerIsNear = false;
            asTalked = false;

            outlineController.outLining = false;
        }

    }
}
