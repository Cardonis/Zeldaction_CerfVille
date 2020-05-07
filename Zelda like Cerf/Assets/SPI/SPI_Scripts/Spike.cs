using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int baseDamage;
    [HideInInspector] public float damagetaken;

    int detectedObjectMass;
    float detectedObjectProjectionLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Elements_Controller element = collision.GetComponentInParent<Elements_Controller>();
        
        Ennemy_Controller ennemyspike = collision.GetComponentInParent<Ennemy_Controller>();
        if (element != null)
        {
            detectedObjectMass = element.mass;
            detectedObjectProjectionLevel = element.levelProjected;
        }
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            collision.attachedRigidbody.GetComponent<Player_Main_Controller>().StartCoroutine(collision.attachedRigidbody.GetComponent<Player_Main_Controller>().TakeDamage(baseDamage));
        }
        if (ennemyspike != null)
        {
            damagetaken = (detectedObjectProjectionLevel * 2) / detectedObjectMass *4;
            collision.GetComponentInParent<Ennemy_Controller>().StartTakeDamage(damagetaken);
        }
    }
}
