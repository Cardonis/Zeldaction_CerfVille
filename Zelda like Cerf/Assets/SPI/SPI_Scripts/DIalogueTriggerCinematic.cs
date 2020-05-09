using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIalogueTriggerCinematic : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Dialogue dialogue;
    void Start()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
