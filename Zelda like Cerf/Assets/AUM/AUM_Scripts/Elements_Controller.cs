using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements_Controller : MonoBehaviour
{
    [HideInInspector] public bool projected = false;
    [HideInInspector] public Rigidbody2D rb;

    public Player_Main_Controller player;

    public float recoveryValue;

    public void TakeForce(Vector2 direction, float forceValue)
    {
        projected = true;

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction * forceValue, ForceMode2D.Impulse);

    }

    public void StartTakeForce(float forceValue)
    {
        StartCoroutine(TakeForce(forceValue));
    }

    public IEnumerator TakeForce(float forceValue)
    {
        projected = true;

        player.projected = true;

        player.rb.velocity = new Vector2(0, 0);

        rb.velocity = new Vector2(0, 0);
        
        

        Vector2 direction = player.transform.position - transform.position;

        Vector2 direction2 = direction;

        Collider2D[] ennemyColliders = GetComponentsInChildren<Collider2D>();


        while(direction.magnitude > 0.7f)
        {
            for(int i = 0; i < ennemyColliders.Length - 1; i++)
            {
                Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), true);
            }
            
            direction = (player.transform.position - transform.position );
            rb.velocity = direction2.normalized * (forceValue / 3);
            yield return null;
        }

        player.projected = false;

        Destroy(GetComponentInChildren<Bullet_Versatil_Controller>().gameObject);

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction2.normalized * forceValue, ForceMode2D.Impulse);

        for(float i = 2.5f; i > 0; i -= Time.fixedDeltaTime)
        {
            yield return null;
        }

        for (int i = 0; i < ennemyColliders.Length - 1; i++)
        {
            Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), false);
        }

    }


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }

    public virtual void FixedUpdate()
    {
        if(projected == true && rb.velocity.magnitude < recoveryValue)
        {
            projected = false;
            rb.velocity = new Vector2(0, 0);
        }
    }

}
