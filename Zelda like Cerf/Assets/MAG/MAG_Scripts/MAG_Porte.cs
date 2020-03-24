using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAG_Porte : MonoBehaviour
{
    public List<MAG_Bouton> connectedButtons = new List<MAG_Bouton>();
    bool isOpen;
    Collider2D colliderPorte;
    void Start()
    {
        colliderPorte = GetComponentInChildren<Collider2D>();
    }

    void Update()
    {
        isOpen = true;
        foreach(MAG_Bouton button in connectedButtons )
        {
            if( button.isPressed == false)
            {
                isOpen = false;
            }
        }

        colliderPorte.enabled = !isOpen;
    }
}
