using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements_Controller : MonoBehaviour
{
    [HideInInspector] public bool projected = false;
    [HideInInspector] public Rigidbody2D rb;


    public float recoveryValue;

    public void TakeForce(Vector2 direction, float forceValue)
    {
        projected = true;

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction * forceValue, ForceMode2D.Impulse);

    }

    public IEnumerator TakeForce(Transform target, float forceValue)
    {
        projected = true;

        rb.velocity = new Vector2(0, 0);
        
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), target.GetComponentInChildren<Collider2D>(), true);

        Vector2 direction = target.position - transform.position;

        while(direction.magnitude > 0.2f)
        {
            direction = (target.position - transform.position );
            rb.velocity = direction.normalized * forceValue;
            yield return null;
        }

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(-direction.normalized * forceValue, ForceMode2D.Impulse);

        for(float i = 2; i>0; i -= Time.fixedDeltaTime)
        {
            yield return null;
        }

        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), target.GetComponentInChildren<Collider2D>(), false);

    }


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        if(projected == true && rb.velocity.magnitude < recoveryValue)
        {
            projected = false;
            rb.velocity = new Vector2(0, 0);
        }
    }

}
