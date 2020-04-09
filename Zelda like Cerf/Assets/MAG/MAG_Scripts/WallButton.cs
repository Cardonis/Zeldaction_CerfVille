using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : Buttonmanager
{
    public int scoreNeeded;
    int detectedObjectMass;
    float detectedObjectProjectionLevel;
    SpriteRenderer buttonSprite;

    void Start()
    {
        isPressed = false;
        buttonSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if(isPressed == false)
        {
            buttonSprite.color = Color.black;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller element = collision.collider.GetComponentInParent<Elements_Controller>();

        if(element != null)
        {
            detectedObjectMass = element.mass;
            detectedObjectProjectionLevel = element.levelProjected;
        }

        if (detectedObjectMass * detectedObjectProjectionLevel >= scoreNeeded)
        {
            isPressed = true;
            buttonSprite.color = Color.green;
        }
    }
}
