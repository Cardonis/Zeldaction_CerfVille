using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int baseDamage;
    [HideInInspector] public float damagetaken;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ennemy_Controller ennemyspike = collision.GetComponentInParent<Ennemy_Controller>();
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            collision.attachedRigidbody.GetComponent<Player_Main_Controller>().StartCoroutine(collision.attachedRigidbody.GetComponent<Player_Main_Controller>().TakeDamage(baseDamage));

            collision.attachedRigidbody.GetComponent<Player_Main_Controller>().StartCoroutine(collision.attachedRigidbody.GetComponent<Player_Main_Controller>().StunnedFor(0.5f));
        }
        if (ennemyspike != null)
        {
            damagetaken = 8;
            ennemyspike.stuned = true;
            ennemyspike.StartTakeDamage(damagetaken, collision.GetComponentInParent<Ennemy_Controller>().velocityBeforeImpactAngle);
            StartCoroutine(AudioManager.instance.PlayOne("Piege_Loup", gameObject));
        }
    }
}
