using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ennemy_Controller : Elements_Controller
{
    [Header("BaseStats")]
    public float pv;
    public float speed;
    public float detectionAngle;
    public bool playerDetected;
    [HideInInspector] public bool marqued;
    [HideInInspector] public float playerAngle;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool attacking = false;
    [HideInInspector] public Coroutine lastAttack;

    public LayerMask detection;

    public List<Ennemy_Controller> ennemyControllersList;

    float cooldownWandering;

    [Header("Wandering")]
    [Range(0.5f, 3f)] public float minWanderingDistance;
    [Range(2f, 6f)] public float maxWanderingDistance;
    bool canWander;
    Vector2 target;
    [HideInInspector] public bool canMove = true;

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
                StartCoroutine(TakeDamage((ec.levelProjected / mass) * ec.mass));
            }
        }        

    }

    public void StartTakeDamage(float damageTaken)
    {
        StartCoroutine(TakeDamage(damageTaken));
    }

    IEnumerator TakeDamage(float damageTaken)
    {
        pv -= damageTaken;

        Color baseColor = GetComponentInChildren<SpriteRenderer>().material.color;

        GetComponentInChildren<SpriteRenderer>().material.color = new Color(255, 0, 0);

        for (float i = 0.1f * damageTaken; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        GetComponentInChildren<SpriteRenderer>().material.color = baseColor;

        if (pv <= 0)
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

    public abstract IEnumerator Attack1();

    public void Wandering()
    {
        if (target != Vector2.zero)
        {
            direction = (target - (Vector2)transform.position).normalized;
        }

        Detection();

        if (Vector2.Distance(target, transform.position) <= 0.1f)
        {
            if (canWander == false)
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

    public void WanderingNewDirection()
    {
        target = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minWanderingDistance, maxWanderingDistance);
        canMove = true;
        canWander = false;
    }

    public void Detection()
    {
        playerAngle = Vector2.Angle(direction, player.transform.position);

        if (playerAngle <= detectionAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Mathf.Min( 5f, Vector2.Distance(transform.position, player.transform.position) ), detection);


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
