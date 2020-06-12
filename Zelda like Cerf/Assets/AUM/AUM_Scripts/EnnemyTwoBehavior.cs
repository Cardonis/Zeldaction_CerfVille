using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 
    EnnemyTwoBehavior : Ennemy_Controller
{
    [Header("Attack 1")]
    public float attackSpeed;
    public float duringAttackSpeed;
    public float attackCooldown;
    float attackCooldownTimer = 0;
    public float distanceToPlayer;
    public float forcePierreLaunch;
    public float forcePlayerLaunch;
    public GameObject pierrePrefab;

    Elements_Controller currentPierre;
    Collider2D currentPierreCol;
    Bullet_Versatil_Controller currentPierreBullet;
    bool launched = false;

    public Collider2D physicCollider;

    Vector2 directionForAttack;

    Vector2 lookDir;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        WanderingNewDirection();

        initialLife = pv;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (projected)
        {
            Ennemy_Controller[] ennemy_Controllers = transform.parent.GetComponentsInChildren<Ennemy_Controller>();

            foreach (Ennemy_Controller ennemy_Controller in ennemy_Controllers)
            {
                ennemy_Controller.canMove = true;

                if (ennemy_Controller.playerDetected == false)
                {
                    ennemy_Controller.playerDetected = true;
                    MusicManager.InBattle = true;
                    MusicManager.EnemyInBattle += 1;
                }
            }

            canMove = true;
            playerDetected = true;
        }

        if (currentPierre != null)
        {
            Physics2D.IgnoreCollision(currentPierreCol, physicCollider, true);

            currentPierre.stuned = true;

            currentPierre.transform.position = transform.position + (player.transform.position - transform.position).normalized * 1f;

            currentPierreBullet = currentPierre.GetComponentInChildren<Bullet_Versatil_Controller>();

            if (currentPierreBullet != null)
            {
                if (currentPierreBullet.levelProjecting >= 3)
                {
                    currentPierre.stuned = false;
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
                currentPierre.stuned = false;
                launched = false;
                currentPierre = null;

                StartCoroutine(ReactivateCollisionWithAfter(currentPierreCol, 0.5f));
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

            

            if (!attacking)
            {
                speed = attackSpeed;

                if (Vector2.Distance(transform.position, player.transform.position) <= distanceToPlayer - 1f)
                {
                    directionForAttack = transform.position - player.transform.position;

                    if (attackCooldownTimer < attackCooldown)
                    {
                        attackCooldownTimer += Time.deltaTime;
                    }

                    for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
                    {
                        outlineController.outLinesAnimator[i].SetBool("IsFallingBack", true);
                    }
                }
                else if (Vector2.Distance(transform.position, player.transform.position) >= distanceToPlayer + 1f)
                {
                    directionForAttack = (player.transform.position - transform.position);

                    for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
                    {
                        outlineController.outLinesAnimator[i].SetBool("IsFallingBack", false);
                    }
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

                if (attackCooldownTimer >= attackCooldown)
                {
                    StartCoroutine(Attack1());
                }

            }                        

        }

        if (canMove)
        {
            rb.AddForce(direction * speed * Time.fixedDeltaTime * 10f, ForceMode2D.Force);

        }

        #region Animator

        lookDir = player.transform.position - transform.position;

        for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
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

        #endregion 
    }

    IEnumerator ReactivateCollisionWithAfter(Collider2D col, float time)
    {
        yield return new WaitForSeconds(time);

        Physics2D.IgnoreCollision(col, physicCollider, false);
    }

    public override IEnumerator Attack1()
    {
        attackCooldownTimer = 0;

        attacking = true;

        speed = duringAttackSpeed;

        List<Elements_Controller> elementsControllers = new List<Elements_Controller>();

        for (int x = 0; x < ennemyControllersList.Count; x++)
        {
            elementsControllers.Add(ennemyControllersList[x]);
        }

        for (int x = 0; x < caisseControllersList.Count; x++)
        {
            elementsControllers.Add(caisseControllersList[x]);
        }

        elementsControllers = elementsControllers.OrderBy(
        x => Vector2.Distance(transform.position, x.transform.position)
        ).ToList();

        if(elementsControllers.Count > 0)
            if (Vector2.Distance(elementsControllers[0].transform.position, transform.position) < Vector2.Distance(player.transform.position, transform.position))
            {
                lastAttack = StartCoroutine(GetAndThrowElement(elementsControllers[0]));
            }
            else
            {
                lastAttack = StartCoroutine(GetAndThrowPlayer());
            }
        else
        {
            lastAttack = StartCoroutine(GetAndThrowPlayer());
        }

        /*Destroy(GetComponent<AudioSource>());
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

        attackCooldownTimer = 0;*/

        yield break;
    }

    public IEnumerator GetAndThrowPlayer()
    {
        telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50f));
        audiomanager.Play("pre-attack");

        canMove = false;

        yield return new WaitForSeconds(0.3f);

        canMove = true;

        bool movingToAttack = true;

        while (movingToAttack == true)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= 1f)
            {
                direction = (transform.position - player.transform.position).normalized;
            }
            else if (Vector2.Distance(transform.position, player.transform.position) >= 2f)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            else
            {
                player.StartCoroutine(player.StunnedFor(0.5f));
                player.stunned = true;

                movingToAttack = false;
                StartCoroutine(audiomanager.PlayOne("Enemy2_Attack", gameObject));
                for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
                {
                    outlineController.outLinesAnimator[i].SetTrigger("Attacks");
                }

                direction = (player.transform.position - transform.position).normalized;
            }


            yield return null;

        }

        canMove = false;
        rb.velocity = Vector2.zero;

        //StartCoroutine(CollisionIgnoreWithElement(1.5f));

        Physics2D.IgnoreCollision(player.physicCollider, col, true);

        Vector2 directionAttack = (player.transform.position - transform.position).normalized;

        for (float x = 1.2f; x > 0.7; x -= Time.deltaTime * 1f)
        {
            if (dead == true || projected == true)
            {
                player.stunned = false;

                attacking = false;

                direction = Vector2.zero;

                canMove = true;

                StopCoroutine(lastAttack);

                yield break;
            }

            rb.velocity = Vector2.zero;

            player.transform.position = (Vector2)transform.position + directionAttack.normalized * x;
            yield return null;
        }

        player.stunned = false;

        player.projected = true;
        player.rb.AddForce(directionAttack.normalized * forcePlayerLaunch, ForceMode2D.Impulse);
        StartCoroutine(audiomanager.PlayOne("Capa_Lance", gameObject));
        StartCoroutine(DontCollideWithPlayerFor(0.2f));

        yield return new WaitForSeconds(0.3f);

        canMove = true;

        direction = Vector2.zero;

        attacking = false;

        yield break;
    }

    public IEnumerator GetAndThrowElement(Elements_Controller currentElement)
    {

        bool movingToAttack = true;

        while (movingToAttack == true)
        {
            if (Vector2.Distance(transform.position, currentElement.transform.position) <= 1f)
            {
                direction = (transform.position - currentElement.transform.position).normalized;
            }
            else if (Vector2.Distance(transform.position, currentElement.transform.position) >= 2.5f)
            {
                direction = (currentElement.transform.position - transform.position).normalized;
            }
            else
            {
                currentPierre = currentElement;

                currentPierreCol = currentPierre.GetComponentInChildren<Collider2D>();

                currentPierre.stuned = true;

                movingToAttack = false;

                for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
                {
                    outlineController.outLinesAnimator[i].SetTrigger("IsThrowingRock");
                }

                direction = (currentElement.transform.position - transform.position).normalized;
            }


            yield return null;

        }

        canMove = false;
        rb.velocity = Vector2.zero;

        attacking = true;

        StartCoroutine(audiomanager.PlayOne("Enemy2_Rock", gameObject));

        for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
        {
            outlineController.outLinesAnimator[i].SetTrigger("Attacks");
        }

        telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50f));
        audiomanager.Play("pre-attack");

        for (float i = 1.5f; i > 0.75f; i -= Time.deltaTime * 1f)
        {
            if (currentPierre == null)
            {
                attacking = false;

                attackCooldownTimer = 0;

                StopCoroutine(lastAttack);
                yield break;
            }

            rb.velocity = Vector2.zero;

            currentPierre.transform.position = (Vector2)transform.position + (Vector2)(player.transform.position - transform.position).normalized * i;
            yield return null;
        }

        if (currentPierre == null)
        {
            attacking = false;

            attackCooldownTimer = 0;

            StopCoroutine(lastAttack);
            yield break;
        }

        StartCoroutine(audiomanager.PlayOne("Enemy2_Attack", gameObject));

        for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
        {
            outlineController.outLinesAnimator[i].SetTrigger("Attacks");
        }

        currentPierre.projected = true;
        currentPierre.levelProjected = 2;

        currentPierre.stuned = false;

        currentPierre.rb.AddForce((player.transform.position - transform.position).normalized * forcePierreLaunch, ForceMode2D.Impulse);

        launched = true;

        direction = Vector2.zero;

        yield return new WaitForSeconds(0.4f);

        attacking = false;

        canMove = true;
    }

}
