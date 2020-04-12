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
    Transform pierresParent;

    Elements_Controller currentPierre;
    Collider2D currentPierreCol;
    Bullet_Versatil_Controller currentPierreBullet;
    bool launched = false;

    public Collider2D physicCollider;

    Vector2 directionForAttack;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();

        WanderingNewDirection();

        pierresParent = transform.parent.parent.Find("Autres");
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (projected)
        {
            playerDetected = true;
        }

        if (currentPierre != null)
        {
            currentPierre.transform.position = transform.position + (player.transform.position - transform.position).normalized * 1.5f;

            currentPierreBullet = currentPierre.GetComponentInChildren<Bullet_Versatil_Controller>();

            if (currentPierreBullet != null)
            {
                if (currentPierreBullet.levelProjecting >= 2)
                {
                    launched = false;
                    carrying = false;
                    currentPierre = null;
                    Physics2D.IgnoreCollision(currentPierreCol, physicCollider, false);
                    attackCooldownTimer = 0;
                }
                else
                {
                    player.GetComponent<Player_Main_Controller>().stunned = false;
                    currentPierre.StopCoroutine(currentPierre.lastTakeForce);
                    Destroy(currentPierreBullet.gameObject);
                }
            }
            else if (launched)
            {
                launched = false;
                carrying = false;
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
            if(caisseControllersList[0] != null)
            {
                if (Vector2.Distance(caisseControllersList[0].transform.position, transform.position) > 4.5f)
                {
                    currentPierre = Instantiate(pierrePrefab, pierresParent).GetComponent<Elements_Controller>();
                }
                else
                {
                    currentPierre = caisseControllersList[0];
                }
            }
            else
            {
                currentPierre = Instantiate(pierrePrefab, pierresParent).GetComponent<Elements_Controller>();
            }
            
            currentPierreCol = currentPierre.GetComponentInChildren<Collider2D>();
            carrying = true;
        }
        else
        {
            attacking = true;

            Vector2 directionAttack = (player.transform.position - transform.position).normalized;

            for(float i = 1.5f; i > 0.75f; i -= Time.deltaTime * 1f)
            {
                currentPierre.transform.position = (Vector2)transform.position + directionAttack * i;
                yield return null;
            }
            

            currentPierre.projected = true;
            currentPierre.rb.AddForce(directionAttack * forcePierreLaunch, ForceMode2D.Impulse);

            attacking = false;

            launched = true;
        }

        attackCooldownTimer = 0;

        yield break;
    }
}
