using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_Controller : Elements_Controller
{
    [Header("BaseStats")]
    public float velocityToGetDamage;
    public float pv;
    public float speed;
    public float detectionAngle;
    [HideInInspector] public bool playerDetected;
    [HideInInspector] public bool marqued;
    [HideInInspector] public float playerAngle;
    [HideInInspector] public Dictionary<float, float> velocityValues = new Dictionary<float, float>();
    [HideInInspector] public Vector2 direction;



    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Elements_Controller ec = collision.transform.GetComponent<Elements_Controller>();

        if (projected == true && levelProjected >= 1)
        {

            if (collision.transform.tag == "Wall")
            {
                StartCoroutine(TakeDamage((levelProjected / mass) * 2));
            }

            if (ec != null)
            {
                StartCoroutine(TakeDamage((levelProjected / mass) * ec.mass));
                
            }
        }

        if(ec != null)
        {
            if(ec.projected && ec.levelProjected >= 1)
            {
                StartCoroutine(TakeDamage((levelProjected / mass) * ec.mass));
            }
        }

        /*
        if (velocityValues.Count > 10)
        if(collision.transform.tag == "Wall" && velocityValues[velocityValues.Count - 10] - rb.velocity.magnitude > velocityToGetDamage)
        {
            StartCoroutine(TakeDamage(2));
               
        }
        */

    }

    public void StartTakeDamage(float damageTaken)
    {
        StartCoroutine(TakeDamage(damageTaken));
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

        if(pv <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        MarquageController mC = GetComponentInChildren<MarquageController>();

        if (mC != null)
        {
            Destroy(mC.gameObject);

            marquageManager.marquageControllers.Remove(mC);
        }

        Destroy(gameObject);
    }


    public void RegisterVelocity()
    {
        velocityValues.Add(velocityValues.Count + 1, rb.velocity.magnitude);
    }

    public void Detection()
    {
        playerAngle = Vector2.Angle(direction, player.transform.position);

        if (playerAngle <= detectionAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position, 5, 8);

            if(hit.collider != null)
            {
                if (hit.collider.attachedRigidbody.transform.tag == "Player")
                {
                    playerDetected = true;
                }
            }
            
        }

    }
}
