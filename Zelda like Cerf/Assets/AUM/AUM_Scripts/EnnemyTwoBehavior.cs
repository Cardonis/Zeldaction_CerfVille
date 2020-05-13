using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 
    EnnemyTwoBehavior : Ennemy_Controller
{
    [Header("Attack 1")]
    public float attackSpeedEnnemyTarget;
    public float attackSpeedPlayerTarget;
    public float attackCooldownCarrying;
    public float attackCooldownNotCarrying;
    float attackCooldown;
    float attackCooldownTimer = 0;
    Transform attackMovementTarget;
    public float distanceToEnnemy;
    public float distanceToPlayer;
    float distanceToTarget;
    public float forcePierreLaunch;
    public GameObject pierrePrefab;
    Transform pierresParent;

    Elements_Controller currentPierre;
    Collider2D currentPierreCol;
    Bullet_Versatil_Controller currentPierreBullet;
    bool launched = false;

    public Collider2D physicCollider;

    Vector2 directionForAttack;

    Vector2 lookDir;

    public Animator animator;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        WanderingNewDirection();

        initialLife = pv;

        pierresParent = transform.parent.parent.Find("Autres");

        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (projected)
        {
            canMove = true;
            playerDetected = true;
        }

        if (currentPierre != null)
        {
            currentPierre.transform.position = transform.position + (player.transform.position - transform.position).normalized * 1f;

            currentPierreBullet = currentPierre.GetComponentInChildren<Bullet_Versatil_Controller>();

            if (currentPierreBullet != null)
            {
                if (currentPierreBullet.levelProjecting >= 2)
                {
                    launched = false;
                    currentPierre = null;
                    Physics2D.IgnoreCollision(currentPierreCol, physicCollider, false);
                    attackCooldownTimer = 0;
                }
                else
                {
                    player.GetComponent<Player_Main_Controller>().stunned = false;

                    
                    currentPierre.StopTakeForce();
                    Destroy(currentPierreBullet.gameObject);
                }
            }
            else if (launched)
            {
                launched = false;
                currentPierre = null;
                Physics2D.IgnoreCollision(currentPierreCol, physicCollider, false);
                attackCooldownTimer = 0;
            }
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

                if (ennemyControllersList[i] == null)
                {
                    ennemyControllersList.RemoveAt(i);
                }
            }

            caisseControllersList = caisseControllersList.OrderBy(
            x => Vector2.Distance(player.transform.position, x.transform.position)
            ).ToList();

            ennemyControllersList = ennemyControllersList.OrderBy(
            x => Vector2.Distance(player.transform.position, x.transform.position)
            ).ToList();

            if(ennemyControllersList.Count > 0)
            {
                speed = attackSpeedEnnemyTarget;
                distanceToTarget = distanceToEnnemy;
                attackMovementTarget = ennemyControllersList[0].transform;
            }
            else
            {
                speed = attackSpeedPlayerTarget;
                distanceToTarget = distanceToPlayer;
                attackMovementTarget = player.transform;
            }

            if (!attacking)
            {
                if (Vector2.Distance(transform.position, attackMovementTarget.position) <= distanceToTarget - 1f)
                {
                    directionForAttack = transform.position - attackMovementTarget.position;
                }
                else if (Vector2.Distance(transform.position, attackMovementTarget.position) >= distanceToTarget + 1f)
                {
                    directionForAttack = (attackMovementTarget.position - transform.position);
                }
                else
                {
                    if (attackCooldownTimer < attackCooldown)
                    {
                        attackCooldownTimer += Time.deltaTime;
                    }

                    directionForAttack = Vector2.zero;
                }

                direction = directionForAttack.normalized;

                if (currentPierre != null)
                {
                    Physics2D.IgnoreCollision(currentPierreCol, physicCollider, true);
                    attackCooldown = attackCooldownCarrying;
                }
                else
                {
                    attackCooldown = attackCooldownNotCarrying;
                }

                if (attackCooldownTimer >= attackCooldown)
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

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        animator.SetFloat("HorizontalAim", lookDir.x);
        animator.SetFloat("VerticalAim", lookDir.y);

        if(rb.velocity != new Vector2(0, 0))
        {
            animator.SetBool("IsMoving", true);
        }

        else
        {
            animator.SetBool("IsMoving", false);
        }

        #endregion 
    }

    public override IEnumerator Attack1()
    {
        Destroy(GetComponent<AudioSource>());
        Destroy(GetComponent<AudioSource>());
        Destroy(GetComponent<AudioSource>());
        if (currentPierre == null)
        {
            if(caisseControllersList.Count != 0)
            {
                if (Vector2.Distance(caisseControllersList[0].transform.position, transform.position) > 4.5f)
                {
                    currentPierre = Instantiate(pierrePrefab, pierresParent).GetComponent<Elements_Controller>();
                    player.confiner.activeRoom.objectsToDestroy.Add(currentPierre);
                    currentPierre.spawned = true;
                    audiomanager.PlayHere("Enemy2_GrabRock", gameObject);

                    animator.SetTrigger("PopingCaillou");
                }
                else
                {
                    currentPierre = caisseControllersList[0];
                    
                }
            }
            else
            {
                currentPierre = Instantiate(pierrePrefab, pierresParent).GetComponent<Elements_Controller>();
                player.confiner.activeRoom.objectsToDestroy.Add(currentPierre);
                currentPierre.spawned = true;
                audiomanager.PlayHere("Enemy2_GrabRock", gameObject);

                animator.SetTrigger("PopingCaillou");
            }
            
            currentPierreCol = currentPierre.GetComponentInChildren<Collider2D>();

        }
        else
        {
            attacking = true;
            
            Vector2 directionAttack = (player.transform.position - transform.position).normalized;
            audiomanager.PlayHere("Enemy2_Rock", gameObject);

            animator.SetTrigger("Attacks");


            for (float i = 1.5f; i > 0.75f; i -= Time.deltaTime * 1f)
            {
                if(currentPierre == null)
                {
                    attacking = false;

                    attackCooldownTimer = 0;

                    StopCoroutine(lastAttack);
                    yield break;
                }

                currentPierre.transform.position = (Vector2)transform.position + directionAttack * i;
                yield return null;
            }

            if (currentPierre == null)
            {
                attacking = false;

                attackCooldownTimer = 0;

                StopCoroutine(lastAttack);
                yield break;
            }

            audiomanager.PlayHere("Enemy2_Attack", gameObject);

            animator.SetTrigger("Attacks");

            currentPierre.projected = true;
            currentPierre.rb.AddForce(directionAttack * forcePierreLaunch, ForceMode2D.Impulse);

            attacking = false;

            launched = true;
        }

        attackCooldownTimer = 0;

        yield break;
    }
}
