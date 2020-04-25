using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiOneBehavior : Ennemy_Controller
{
    [Header("Attack 1")]
    public float attackSpeed;
    public float attackCooldown;
    float attackCooldownTimer = 0;
    public Collider2D attackCollider;
    public Collider2D physicCollider;
    public float attackForce;
    [HideInInspector] public bool hasAttacked = false;
    bool nobodyHasAttacked;

    [HideInInspector] public Vector2 attackDirection;

    Vector2 directionForAttack;

    public List<EnnemiOneBehavior> limierControllers;

    AudioManager audiomanager;

    [HideInInspector] public Animator animator;

    override public void Start()
    {
        base.Start();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        attackCollider.enabled = false;
        WanderingNewDirection();

        initialLife = pv;

        limierControllers.Clear();

        for(int i = 0; i < transform.parent.childCount; i++)
        {
            EnnemiOneBehavior eob = transform.parent.GetChild(i).GetComponent<EnnemiOneBehavior>();
            if (eob != null)
            {
                limierControllers.Add(eob);
            }
        }

        animator = GetComponentInChildren<Animator>();
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
            Wandering();
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
                
                if (Vector2.Distance(transform.position, player.transform.position) <= 3f)
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
                    if(ennemyControllersList[i].GetComponent<EnnemiOneBehavior>() != null)
                    directionForAttack = directionForAttack + ((Vector2)transform.position - (Vector2)ennemyControllersList[i].transform.position);
                }

                direction = directionForAttack.normalized;

                for (int i = 0; i < limierControllers.Count; i++)
                {
                    if (limierControllers[i].hasAttacked == true && limierControllers[i].gameObject.activeSelf == true)
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

        #region Animator

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);


        #endregion Animator
    }

    public override IEnumerator Attack1()
    {
        StartCoroutine(HasAttackedFor(1.2f));
        attackCooldownTimer = 0;

        attacking = true;

        attackDirection = player.transform.position - transform.position;

        rb.velocity = new Vector2(0, 0);
        audiomanager.PlayHere("Enemy1_grognement", gameObject);
        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            rb.velocity = -attackDirection.normalized * 50f * Time.fixedDeltaTime;
            
            yield return null;
        }
        
        StartCoroutine(HitboxAttackActivatedFor(1.5f));

        Destroy(GetComponent<AudioSource>());
        audiomanager.PlayHere("Enemy1_Attack", gameObject);

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
