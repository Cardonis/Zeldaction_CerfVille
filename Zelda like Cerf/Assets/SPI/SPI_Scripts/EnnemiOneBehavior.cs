using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiOneBehavior : Ennemy_Controller
{
    [HideInInspector]

    public float ennemiSpeed;
    /*public float ennemiDamage;
    public int ennemiLife;
    public int ennemiLifeLeft;*/

    public float kockbackDistance;

    public float distanceToDash;
    public float timeDashCharge;
    public float recoveryTime;

    public float dashSpeed;

    private Vector3 targetPosition;
    Vector2 target;

    [HideInInspector] public bool canMove=true;
    [HideInInspector]public bool canDash = true;

    [HideInInspector] public Color dashColor = Color.red;
    [HideInInspector] public Color normalColor = Color.white;

    [Header("Wandering")]
    float cooldownWandering;
    public float minWanderingDistance;
    public float maxWanderingDistance;
    bool canWander;





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
            if (Vector2.Distance(transform.position, player.transform.position) > distanceToDash && canMove == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, ennemiSpeed * Time.deltaTime);
            }

            else if (Vector2.Distance(transform.position, player.transform.position) <= distanceToDash && canMove == true)
            {
                canMove = false;

                StartCoroutine("EnnemiDash");
            }
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

    private void OnTriggerEnter2D(Collider2D other)
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
}
