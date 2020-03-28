using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedPillar : MonoBehaviour
{
    public float wallDamagePoints;
    public int wallMaxPoints;
    public Vector2 buttonSize;
    int detectedObjectMass;
    float detectedObjectProjectionLevel;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller element = collision.collider.GetComponentInParent<Elements_Controller>();
        
        if (element != null)
        {
            detectedObjectMass = element.mass;
            detectedObjectProjectionLevel = element.levelProjected;
            wallDamagePoints += detectedObjectMass * detectedObjectProjectionLevel;
            
        }
        if (wallDamagePoints >= wallMaxPoints)
        {

            gameObject.SetActive(false);
        }
    }
}
