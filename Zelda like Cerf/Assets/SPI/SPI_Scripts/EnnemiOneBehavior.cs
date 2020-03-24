using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiOneBehavior : Ennemy_Controller
{
    float cooldownWandering;

    [Header("Wandering")]
    [Range(0.5f,3f)]public float minWanderingDistance;
    [Range(2f, 6f)]public float maxWanderingDistance;
    bool canWander;

    [Header("Attack 1")]
    [Range(0.5f, 5f)] public float kockbackDistance;
    [Range(0.5f, 5f)] public float distanceToDash;
    [Range(0f, 10f)] public float timeDashCharge;
    [Range(0f, 10f)] public float recoveryTime;
    [Range(1f, 12f)] public float dashSpeed;

    [HideInInspector]public bool canMove=true;
    [HideInInspector]public bool canDash = true;

    [HideInInspector] public Color dashColor = Color.red;
    [HideInInspector] public Color normalColor = Color.white;

    private Vector3 targetPosition;
    Vector2 target;


    override public void Start()
    {
        base.Start();

        playerDetected = false;
        WanderingNewDirection();
    }


    override public void FixedUpdate()
    {
        base.FixedUpdate();

        if (projected)
        {
            RegisterVelocity();
            return;
        }
        else
        {
            velocityValues.Clear();
        }
          
        

        if (playerDetected == false)
        {
            
            if (target != Vector2.zero)
            {
                direction = (target - (Vector2)transform.position).normalized;
            }
            Detection();

            if (Vector2.Distance(target, transform.position) <= 0.1f)
            {
                if(canWander == false)
                {
                    cooldownWandering = Random.Range(1f, 2f);
                    canWander = true;
                }
                else
                {
                    canMove = false;

                    cooldownWandering -= Time.deltaTime;

                    if (cooldownWandering <= 0)
                    {
                        WanderingNewDirection();
                    }
                }
            }


        }
        else
        {
            /*if (Vector2.Distance(transform.position, player.transform.position) > distanceToDash && canMove == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }

            else if (Vector2.Distance(transform.position, player.transform.position) <= distanceToDash && canMove == true)
            {
                canMove = false;

                StartCoroutine("EnnemiDash");
            }*/

            for(int i = 0; i < ennemyControllersList.Count; i++)
            {
                if (ennemyControllersList[i].GetComponent <EnnemiOneBehavior>())
                {

                }
            }


            direction = (target - (Vector2)transform.position).normalized;
        }
        if (canMove)
        {
            rb.velocity = direction * speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
            
    }

    IEnumerator EnnemiDash()
    {
        GetComponentInChildren<SpriteRenderer>().material.color = dashColor;
        yield return new WaitForSeconds(timeDashCharge);
        GetComponentInChildren<SpriteRenderer>().material.color = normalColor;
        targetPosition = new Vector2(player.transform.position.x, player.transform.position.y);

        while (transform.position != targetPosition && canDash == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.005f);
        }
        canDash = true;
        yield return new WaitForSeconds(recoveryTime);
        canMove = true;
    }

    public void WanderingNewDirection()
    {
        target = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,1f)).normalized *Random.Range(minWanderingDistance,maxWanderingDistance);
        canMove = true;
        canWander = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            canDash = true;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canDash = true;
            
        }
    }

    public void BehaviorWithEnnemi1()
    {
        //direction = (target - (Vector2)transform.position +).normalized;
    }
}
