using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnnemyTwoBehavior : Ennemy_Controller
{
    [Header("Attack 1")]
    public float attackSpeedEnnemyTarget;
    public float attackSpeedPlayerTarget;
    public float attackCooldownCarrying;
    public float attackCooldownNotCarrying;
    float attackCooldown;
    float attackCooldownTimer = 0;
    public bool carrying;
    Transform attackMovementTarget;
    public float distanceToEnnemy;
    public float distanceToPlayer;
    float distanceToTarget;
    public float forcePierreLaunch;
    public GameObject pierrePrefab;
    public Transform pierresParent;

    Elements_Controller currentPierre;
    Collider2D currentPierreCol;

    public Collider2D physicCollider;

    Vector2 directionForAttack;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();

        WanderingNewDirection();
    }

    // Update is called once per frame
    public override void FixedUpdate()
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

                if (ennemyControllersList[i] == null)
                {
                    ennemyControllersList.RemoveAt(i);
                }
            }

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

                if (carrying)
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

            if (currentPierre != null)
            {
                currentPierre.transform.position = transform.position + (player.transform.position - transform.position).normalized * 1.5f;

                if (currentPierre.projected == true)
                {
                    carrying = false;
                    currentPierre = null;
                    Physics2D.IgnoreCollision(currentPierreCol, physicCollider, false);
                    attackCooldownTimer = 0;
                }
            }
            

        }

        if (canMove && attacking == false)
        {
            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }
    }

    public override IEnumerator Attack1()
    {
        if(!carrying)
        {
            currentPierre = Instantiate(pierrePrefab, pierresParent).GetComponent<Elements_Controller>();
            currentPierreCol = currentPierre.GetComponentInChildren<Collider2D>();
            carrying = true;

        }
        else
        {
            attacking = true;

            Vector2 directionAttack = (player.transform.position - transform.position).normalized;

            for(float i = 1.5f; i > 0.75f; i -= Time.deltaTime * 0.75f)
            {
                currentPierre.transform.position = (Vector2)transform.position + directionAttack * i;
                yield return null;
            }
            

            currentPierre.projected = true;
            currentPierre.rb.AddForce(directionAttack * forcePierreLaunch, ForceMode2D.Impulse);

            attacking = false;
        }

        attackCooldownTimer = 0;

        yield break;
    }
}
