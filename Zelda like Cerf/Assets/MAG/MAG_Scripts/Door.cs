using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Buttonmanager> normalyConnectedButtons = new List<Buttonmanager>();
    bool isOpen;
    Collider2D doorCollider;
    SpriteRenderer doorSprite;
    public TimeConnectors timeConnectors;
    public OrderConnectors orderConnectors;
    void Start()
    {
        doorCollider = GetComponentInChildren<Collider2D>();
        doorSprite = GetComponentInChildren<SpriteRenderer>();
        
    }

    void Update()
    {
        isOpen = false;
        foreach(Buttonmanager button in normalyConnectedButtons )
        {
            if( button.isPressed == false)
            {
                isOpen = true;
            }
        }
        
        if(timeConnectors != null)
            if(timeConnectors.openDoor == true)
            {
                isOpen = true;
            }

        if (orderConnectors != null)
            if (orderConnectors.openDoor == true)
            {
                isOpen = true;
            }

        doorCollider.enabled = !isOpen;
        doorSprite.enabled = !isOpen;
    }
}
