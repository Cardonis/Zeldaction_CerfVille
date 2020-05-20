using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dialogueManager;
    bool playerIsNear;
    Player_Main_Controller player;

    public OutlineController outlineController;

    public bool asTalked;

    private void Start()
    {
        playerIsNear = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller p = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if(p != null)
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

            outlineController.outLining = false;
        }

    }
    private void Update()
    {
        
        if (Input.GetButtonDown("X") && dialogueManager.dialogueStarted == false && playerIsNear == true)
        {
            asTalked = true;
            player.pressX.SetActive(false);
            dialogueManager.StartDialogue(dialogue);

            outlineController.outLining = false;
        }
    }
}
