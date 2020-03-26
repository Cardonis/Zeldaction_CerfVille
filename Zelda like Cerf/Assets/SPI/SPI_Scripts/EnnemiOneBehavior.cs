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
    public float attackSpeed;
    public float attackCooldown;
    float attackCooldownTimer = 0;
    public Collider2D attackCollider;
    public Collider2D physicCollider;
    public float attackForce;
    [HideInInspector] public bool hasAttacked = false;
    bool nobodyHasAttacked;

    [HideInInspector] public bool canMove = true;
    [HideInInspector]public bool canDash = true;

    [HideInInspector] public Color dashColor = Color.red;
    [HideInInspector] public Color normalColor = Color.white;

    private Vector3 targetPosition;
    Vector2 target;

    [HideInInspector] public Vector2 attackDirection;

    Vector2 directionForAttack;

    public List<EnnemiOneBehavior> limierControllers;

    override public void Start()
    {
        base.Start();

        attackCollider.enabled = false;

        playerDetected = false;
        WanderingNewDirection();
    }


    override public void FixedUpdate()
    {
        base.FixedUpdate();

        if (projected)
        {
            playerDetected = true;
        }

        if (stuned == true || projected == true)
        {
            return;
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
            for (int i = 0; i < ennemyControllersList.Count; i++)
            {
                ennemyControllersList[i].playerDetected = true;
            }

            if (!attacking)
            {
                speed = attackSpeed;

                if(Vector2.Distance(transform.position, player.transform.position) <= 3f)
                {
                    directionForAttack = transform.position - player.transform.position;
                }
                else if(Vector2.Distance(transform.position, player.transform.position) >= 4.5f)
                {
                    directionForAttack = (player.transform.position - transform.position) * 1.5f;
                }
                else
                {
                    if(attackCooldownTimer < attackCooldown)
                    {
                        attackCooldownTimer += Time.deltaTime;
                    }
                    
                    directionForAttack = Vector2.zero;
                }

                nobodyHasAttacked = true;

                for (int i = 0; i < ennemyControllersList.Count; i++)
                {
                    directionForAttack = directionForAttack + ((Vector2)transform.position - (Vector2)ennemyControllersList[i].transform.position);
                }

                direction = directionForAttack.normalized;

                for (int i = 0; i < limierControllers.Count; i++)
                {
                    if (limierControllers[i].hasAttacked == true)
                    {
                        nobodyHasAttacked = false;
                    }
                }

                if(attackCooldownTimer >= attackCooldown && nobodyHasAttacked)
                {
                    lastAttack = StartCoroutine(Attack1());
                }

            }

        }

        if (canMove && attacking == false)
        {
            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }
            
    }

    public void WanderingNewDirection()
    {
        target = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,1f)).normalized *Random.Range(minWanderingDistance,maxWanderingDistance);
        canMove = true;
        canWander = false;
    }

    public override IEnumerator Attack1()
    {
        StartCoroutine(HasAttackedFor(1.2f));
        attackCooldownTimer = 0;

        attacking = true;

        attackDirection = player.transform.position - transform.position;

        rb.velocity = new Vector2(0, 0);

        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            rb.velocity = -attackDirection.normalized * 50f * Time.fixedDeltaTime;
            yield return null;
        }

        StartCoroutine(HitboxAttackActivatedFor(1.5f));

        rb.velocity = new Vector2(0, 0);

        rb.AddForce( (attackDirection).normalized * attackForce, ForceMode2D.Impulse );
        projected = true;

        while (projected == true)
        {
            yield return null;
        }

        attackCooldownTimer = 0;
        attacking = false;

        Physics2D.IgnoreCollision(physicCollider, player.physicCollider, false);
        attackCollider.enabled = false;

        yield break;
    }

    public IEnumerator HitboxAttackActivatedFor(float time)
    {
        Physics2D.IgnoreCollision(physicCollider, player.physicCollider, true);
        attackCollider.enabled = true;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Physics2D.IgnoreCollision(physicCollider, player.physicCollider, false);
        attackCollider.enabled = false;
    }

    public IEnumerator HasAttackedFor(float time)
    {
        hasAttacked = true;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        hasAttacked = false;
    }

}
