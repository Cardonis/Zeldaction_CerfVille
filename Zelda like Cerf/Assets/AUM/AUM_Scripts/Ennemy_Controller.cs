using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ennemy_Controller : Elements_Controller
{
    [Header("BaseStats")]
    public float pv;
    [HideInInspector] public float initialLife;
    public float speed;
    public float detectionAngle;
    public float detectionDistance;
    public bool playerDetected;
    [HideInInspector] public bool marqued;
    [HideInInspector] public float playerAngle;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool attacking = false;
    [HideInInspector] public Coroutine lastAttack;

    [HideInInspector] public bool dead = false;

    public LayerMask detection;

    public List<Ennemy_Controller> ennemyControllersList;
    public List<Caisse_Controller> caisseControllersList;

    float cooldownWandering;

    [Header("Wandering")]
    [Range(0.5f, 3f)] public float minWanderingDistance;
    [Range(2f, 6f)] public float maxWanderingDistance;
    bool canWander;
    public Vector2 target;
    public bool canMove = true;
    float wanderingTime;

    public Collider2D col;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Elements_Controller ec = collision.transform.GetComponent<Elements_Controller>();

        if (projected == true && levelProjected >= 0.5f)
        {

            if (collision.transform.tag == "Wall" || collision.transform.tag == "Ronce")
            {
                StartCoroutine(TakeDamage((levelProjected / mass) * 1.5f));
            }

            if (ec != null)
            {
                StartCoroutine(TakeDamage((levelProjected / mass) * ec.mass));
                
            }
        }

        if(ec != null)
        {
            if(ec.projected && ec.levelProjected >= 0.5f)
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

        dead = true;

        gameObject.SetActive(false);
    }

    public abstract IEnumerator Attack1();

    public void Wandering()
    {
        if (canMove == true)
        {
            direction = (target - (Vector2)transform.position).normalized;
        }
        else
        {
            direction = Vector2.zero;
        }

        if (wanderingTime >= 0)
        {
            wanderingTime -= Time.deltaTime;
        }

        Detection();

        if (Vector2.Distance(target, transform.position) <= 0.1f || wanderingTime < 0)
        {
            if (canWander == false)
            {
                cooldownWandering = Random.Range(1f, 2f);
                canWander = true;
                canMove = false;
            }
            else
            {

                cooldownWandering -= Time.deltaTime;

                if (cooldownWandering <= 0)
                {
                    WanderingNewDirection();
                    wanderingTime = Random.Range(1f, 2f);
                }
            }
        }
    }

    public void WanderingNewDirection()
    {
        target = (new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized * Random.Range(minWanderingDistance, maxWanderingDistance)) + (Vector2)transform.position;
        canWander = false;
        canMove = true;
    }

    public void Detection()
    {
        playerAngle = Vector2.Angle(direction, player.transform.position - transform.position);


        if (playerAngle <= detectionAngle)
        {
            col.enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position,Mathf.Infinity , detection);
            col.enabled = true;

            if(hit.collider.attachedRigidbody != null)
            {
                if(hit.distance <= detectionDistance)
                if (hit.collider.attachedRigidbody.transform.tag == "Player")
                {
                    playerDetected = true;
                    canMove = true;
                }
            }
            
        }

    }

}
