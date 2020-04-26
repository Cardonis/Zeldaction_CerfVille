using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dialogueManager;
    bool playerIsNear;
    Player_Main_Controller player;
    public GameObject pressX;

    private void Start()
    {
        playerIsNear = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        pressX.SetActive(true);
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if(player != null)
        {
            playerIsNear = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            pressX.SetActive(false);
            playerIsNear = false;
        }

    }
    private void Update()
    {
        if (Input.GetButtonDown("X") && dialogueManager.dialogueStarted == false && playerIsNear == true)
        {
            pressX.SetActive(false);
            dialogueManager.StartDialogue(dialogue);

        }
    }
}
