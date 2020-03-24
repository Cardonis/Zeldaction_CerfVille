using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAG_BoutonMural : MAG_Bouton
{
    public int scoreNeeded;
    public Vector2 buttonSize;
    int detectedObjectMass;
    float detectedObjectProjectionLevel;
    List<Collider2D> pierreColliders = new List<Collider2D>();
    ContactFilter2D pierreFilter = new ContactFilter2D();

    void Start()
    {
        isPressed = false;
        pierreFilter.useTriggers = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller element = collision.collider.GetComponentInParent<Elements_Controller>();

        if(element != null)
        {
            detectedObjectMass = element.mass;
            detectedObjectProjectionLevel = element.levelProjected;
        }

        if (detectedObjectMass * detectedObjectProjectionLevel > scoreNeeded)
        {
            isPressed = true;
            Debug.Log("isPressed");
        }
    }
}
