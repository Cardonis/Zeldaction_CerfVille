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

    Vector2 lookDir;

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
    }


    override public void FixedUpdate()
    {
        base.FixedUpdate();

        if (projected)
        {
            Ennemy_Controller[] ennemy_Controllers = transform.parent.GetComponentsInChildren<Ennemy_Controller>();

            foreach (Ennemy_Controller ennemy_Controller in ennemy_Controllers)
            {
                ennemy_Controller.playerDetected = true;
                ennemy_Controller.canMove = true;
                MusicManager.InBattle = true;
                MusicManager.EnemyInBattle += 1;
            }

            canMove = true;
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
                    directionForAttack = directionForAttack + ((Vector2)transform.position - (Vector2)ennemyControllersList[i].transform.position) * 2;
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

        lookDir = player.transform.position - transform.position;

        for(int i = 0; i < outlineController.outLinesAnimator.Count; i++)
        {
            outlineController.outLinesAnimator[i].SetFloat("Horizontal", direction.x);
            outlineController.outLinesAnimator[i].SetFloat("Vertical", direction.y);

            outlineController.outLinesAnimator[i].SetFloat("HorizontalAim", lookDir.x);
            outlineController.outLinesAnimator[i].SetFloat("VerticalAim", lookDir.y);

            if (rb.velocity != new Vector2(0, 0))
            {
                outlineController.outLinesAnimator[i].SetBool("IsMoving", true);
            }
            else
            {
                outlineController.outLinesAnimator[i].SetBool("IsMoving", false);
            }
        }

        #endregion Animator
    }

    public override IEnumerator Attack1()
    {
        StartCoroutine(HasAttackedFor(1.2f));
        attackCooldownTimer = 0;

        attacking = true;

        rb.velocity = new Vector2(0, 0);
        StartCoroutine(audiomanager.PlayOne("Enemy1_grognement", gameObject));

        telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50f));

        for (float i = 0.4f; i > 0; i -= Time.deltaTime)
        {
            for (int y = 0; y < outlineController.outLinesAnimator.Count; y++)
            {
                outlineController.outLinesAnimator[y].SetBool("IsCharging", true);
            }
            rb.velocity = -(player.transform.position - transform.position).normalized * 50f * Time.fixedDeltaTime;
            
            yield return null;
        }

        StartCoroutine(HitboxAttackActivatedFor(1.5f));

        StartCoroutine(audiomanager.PlayOne("Enemy1_Attack", gameObject));

        for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
        {
            outlineController.outLinesAnimator[i].SetBool("IsCharging", false);
        }

        rb.velocity = new Vector2(0, 0);

        for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
        {
            outlineController.outLinesAnimator[i].SetTrigger("Attacks");
        }

        attackDirection = player.transform.position - transform.position;

        rb.AddForce( attackDirection.normalized * attackForce, ForceMode2D.Impulse );
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
