using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAG_BoutonMural : MAG_Bouton
{
    // Start is called before the first frame update
    public int scoreNeeded;
    public Vector2 buttonSize;
    int detectedObjectMass;
    float detectedObjectProjectionLevel;
    ContactFilter2D pierreFilter = new ContactFilter2D();
    public LayerMask pierreLayerMask;
    void Start()
    {
        isPressed = false;
        pierreFilter.SetLayerMask(LayerMask.GetMask("Pierre"));
        pierreFilter.useTriggers = true;
    }

    void Update()
    {
      Collider2D detectedObject = Physics2D.OverlapBox(transform.position, buttonSize, 0, pierreLayerMask);
      detectedObjectMass = detectedObject.GetComponentInParent<Caisse_Controller>().mass;
      detectedObjectProjectionLevel = detectedObject.GetComponentInParent<Caisse_Controller>().levelProjected;

    if(detectedObjectMass*detectedObjectProjectionLevel > scoreNeeded )
        {
            isPressed = true;
            Debug.Log("isPressed");
        }
    }
}
