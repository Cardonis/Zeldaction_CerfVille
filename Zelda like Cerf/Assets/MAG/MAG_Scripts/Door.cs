using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Buttonmanager> connectedButtons = new List<Buttonmanager>();
    bool isOpen;
    Collider2D colliderPorte;
    void Start()
    {
        colliderPorte = GetComponentInChildren<Collider2D>();
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

        colliderPorte.enabled = !isOpen;
    }
}
