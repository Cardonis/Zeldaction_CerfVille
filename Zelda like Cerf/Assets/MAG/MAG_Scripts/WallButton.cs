using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : Buttonmanager
{
    public int scoreNeeded;
    public Vector2 buttonSize;
    int detectedObjectMass;
    float detectedObjectProjectionLevel;

    void Start()
    {
        isPressed = false;
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
