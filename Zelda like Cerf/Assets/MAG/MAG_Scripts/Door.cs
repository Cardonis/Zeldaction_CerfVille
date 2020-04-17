using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Buttonmanager> normalyConnectedButtons = new List<Buttonmanager>();
    [HideInInspector] public bool isOpen;
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
        isOpen = true;
        foreach(Buttonmanager button in normalyConnectedButtons )
        {
            if( button.isPressed == false)
            {
                isOpen = false;
            }
        }
        
        if(timeConnectors != null)
            if(timeConnectors.openDoor == false)
            {
                isOpen = false;
            }

        if (orderConnectors != null)
            if (orderConnectors.openDoor == false)
            {
                isOpen = false;
            }

        doorCollider.enabled = !isOpen;
        doorSprite.enabled = !isOpen;
    }
}
