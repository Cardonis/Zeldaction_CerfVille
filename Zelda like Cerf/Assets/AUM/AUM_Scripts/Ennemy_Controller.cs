using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_Controller : Elements_Controller
{
    public float velocityToGetDamage;

    public float pv;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if(collision.transform.tag == "Wall" && rb.velocity.magnitude > velocityToGetDamage)
        {

            Debug.Log("Oui");
            StartCoroutine(TakeDamage(2));
        }
    }

    IEnumerator TakeDamage(float damageTaken)
    {
        pv -= damageTaken;

        GetComponentInChildren<SpriteRenderer>().material.color = new Color(255, 0, 0);

        for(float i = 1; i > 0; i -= Time.fixedDeltaTime)
        {
            yield return null;
        }

        GetComponentInChildren<SpriteRenderer>().material.color = new Color(183, 183, 183);

        yield break;
    }

}
