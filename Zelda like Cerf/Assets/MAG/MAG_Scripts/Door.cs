using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Buttonmanager> connectedButtons = new List<Buttonmanager>();
    bool isOpen;
    Collider2D doorCollider;
    SpriteRenderer doorSprite;
    void Start()
    {
        doorCollider = GetComponentInChildren<Collider2D>();
        doorSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        isOpen = true;
        foreach(Buttonmanager button in connectedButtons )
        {
            if( button.isPressed == false)
            {
                isOpen = false;
            }
        }

        doorCollider.enabled = !isOpen;
        doorSprite.enabled = !isOpen;
    }
}
