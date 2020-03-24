using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAG_BoutonSol : MAG_Bouton
{
    public int totalMass;
    public Vector2 buttonSize;
    List<Collider2D> pierreColliders = new List<Collider2D>();
    ContactFilter2D pierreFilter = new ContactFilter2D();
    int currentPressionMass;
    void Start()
    {
        pierreFilter.SetLayerMask(LayerMask.GetMask("Pierre"));
        pierreFilter.useTriggers = true;
        isPressed = false;
    }

    void Update()
    {
        Physics2D.OverlapBox(transform.position, buttonSize, 0, pierreFilter, pierreColliders);
        currentPressionMass = 0;
        for(int i=0; i<pierreColliders.Count; i++)
        {
            currentPressionMass += pierreColliders[i].GetComponentInParent<Caisse_Controller>().mass; 
        }
        if (currentPressionMass >= totalMass)
        {
            isPressed = true;
            Debug.Log(isPressed);
        }
    }
}
