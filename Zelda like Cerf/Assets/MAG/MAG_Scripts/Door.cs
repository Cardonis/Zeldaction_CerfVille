using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Buttonmanager> connectedButtons = new List<Buttonmanager>();
    bool isOpen;
    Collider2D doorCollider;
    SpriteRenderer doorSprite;
    public TimeConnectors timeConnectors;
    void Start()
    {
        doorCollider = GetComponentInChildren<Collider2D>();
        doorSprite = GetComponentInChildren<SpriteRenderer>();
        
    }

    void Update()
    {
        isOpen = false;
        foreach(Buttonmanager button in connectedButtons )
        {
            if( button.isPressed == false)
            {
                isOpen = true;
            }
        }
        
        if(timeConnectors.openDoor == true) {
            isOpen = true;
        }
        doorCollider.enabled = !isOpen;
        doorSprite.enabled = !isOpen;
    }
}
