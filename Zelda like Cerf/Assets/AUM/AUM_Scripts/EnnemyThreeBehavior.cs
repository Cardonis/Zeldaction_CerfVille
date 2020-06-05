using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyThreeBehavior : Ennemy_Controller
{
    [Header("Attack 1")]
    public GameObject bulletAttack1;
    public float attackSpeed;
    public float attackCooldown;
    float attackCooldownTimer = 0;
    public float attackRange;
    public float attackRangeToPlayer;
    public Collider2D physicCollider;
    public float attackForce;
    public float attackForcePlayer;
    public float attackBulletForce;

    Vector2 directionForAttack;

    Vector2 lookDir;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();

        WanderingNewDirection();

        initialLife = pv;

        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    override public void FixedUpdate()
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
            if (!attacking)
            {
                speed = attackSpeed;

                if (Vector2.Distance(transform.position, player.transform.position) <= attackRangeToPlayer)
                {
                    directionForAttack = (transform.position - player.transform.position).normalized * (1f + attackRangeToPlayer - Vector2.Distance(transform.position, player.transform.position) );
                    for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
                    {
                        outlineController.outLinesAnimator[i].SetBool("IsFallingBack", true);
                    }
                }
                else if ( Vector2.Distance(transform.position, player.transform.position) >= (attackRangeToPlayer + 1) )
                {
                    directionForAttack = (player.transform.position - transform.position).normalized;
                    for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
                    {
                        outlineController.outLinesAnimator[i].SetBool("IsFallingBack", false);
                    }
                }
                else
                {

                    directionForAttack = Vector2.zero;
                }

                if (attackCooldownTimer < attackCooldown)
                {
                    attackCooldownTimer += Time.deltaTime;
                }

                direction = directionForAttack;
            }

            if (attackCooldownTimer >= attackCooldown)
            {
                lastAttack = StartCoroutine(Attack1());
            }
        }

        if (canMove && attacking == false)
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

            if (Mathf.Approximately(rb.velocity.x, 0) && Mathf.Approximately(rb.velocity.y, 0))
            {
                outlineController.outLinesAnimator[i].SetBool("IsMoving", false);
            }

            else
            {
                outlineController.outLinesAnimator[i].SetBool("IsMoving", true);
            }
        }

        #endregion Animator

    }

    public override IEnumerator Attack1()
    {
        stuned = true;

        telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50));

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(audiomanager.PlayOne("Enemy3Attack", gameObject));
        BulletAttack1 bullet = Instantiate(bulletAttack1, player.vBullets).GetComponent<BulletAttack1>();

        Collider2D[] ennemyColliders = GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
        {
            outlineController.outLinesAnimator[i].SetTrigger("Attacks");
        }

        for (int i = 0; i < ennemyColliders.Length - 1; i++)
        {
            Physics2D.IgnoreCollision(ennemyColliders[i], bullet.GetComponentInChildren<Collider2D>(), true);
        }

        bullet.transform.position = transform.position;

        //projected = true;
        //rb.velocity = new Vector2(0, 0);

        bullet.ennemyController = transform;
        bullet.maxDistance = attackRange;

        bullet.rb.velocity = (player.transform.position - transform.position).normalized * attackBulletForce * Time.fixedDeltaTime;

        attackCooldownTimer = 0;

        yield break;
    }

    public IEnumerator ApplyForce(Player_Main_Controller player)
    {
        stuned = true;

        player.projected = true;

        Vector2 direction = transform.position - player.transform.position;

        int originalPlayerLayer = player.physicCollider.gameObject.layer;

        player.physicCollider.gameObject.layer = 15;

        //StartCoroutine(DontCollideWithPlayerFor(1f));

        while (direction.magnitude > 0.7f)
        {

            direction = (transform.position - player.transform.position);
            player.rb.velocity = direction.normalized * attackForcePlayer;

            if(direction.magnitude < 1.5f)
            {
                for (int i = 0; i < collider2Ds.Count; i++)
                {
                    Physics2D.IgnoreCollision(collider2Ds[i], player.physicCollider, true);
                }

                player.physicCollider.gameObject.layer = originalPlayerLayer;
            }
                

            yield return null;
        }

        stuned = false;

        if (player.GetComponentInChildren<BulletAttack1>() != null)
            Destroy(player.GetComponentInChildren<BulletAttack1>().gameObject);

        player.rb.velocity = new Vector2(0, 0);

        player.rb.AddForce(direction.normalized * attackForcePlayer * 5, ForceMode2D.Impulse);

        player.physicCollider.gameObject.layer = originalPlayerLayer;

        StartCoroutine(DontCollideWithPlayerFor(0.3f));
    }

    public IEnumerator ApplyForce(Elements_Controller elementsController)
    {
        stuned = true;

        elementsController.projected = true;

        Vector2 direction = transform.position - elementsController.transform.position;

        for (int i = 0; i < elementsController.collider2Ds.Count; i++)
        {
            elementsController.collider2Ds[i].gameObject.layer = 15;
        }

        while (direction.magnitude > 0.7f)
        {
            direction = (transform.position - elementsController.transform.position);
            elementsController.rb.velocity = direction.normalized * attackForce;

            if (direction.magnitude < 1.5f)
            {
                for (int i = 0; i < collider2Ds.Count; i++)
                {
                    for (int x = 0; x < elementsController.collider2Ds.Count; x++)
                        Physics2D.IgnoreCollision(collider2Ds[i], elementsController.collider2Ds[x], true);
                }

                for (int i = 0; i < elementsController.collider2Ds.Count; i++)
                {
                    elementsController.collider2Ds[i].gameObject.layer = 15;
                }
            }

            yield return null;
        }

        stuned = false;

        if (elementsController.GetComponentInChildren<BulletAttack1>() != null)
            Destroy(elementsController.GetComponentInChildren<BulletAttack1>().gameObject);

        elementsController.rb.velocity = new Vector2(0, 0);

        elementsController.rb.AddForce(direction.normalized * attackForce * 3, ForceMode2D.Impulse);

        elementsController.levelProjected = 2;

        for (int i = 0; i < elementsController.collider2Ds.Count; i++)
        {
            elementsController.collider2Ds[i].gameObject.layer = elementsController.ennemyCollidersLayers[i];
        }

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            for (int x = 0; x < elementsController.collider2Ds.Count; x++)
                Physics2D.IgnoreCollision(collider2Ds[i], elementsController.collider2Ds[x], true);
        }

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            for (int x = 0; x < elementsController.collider2Ds.Count; x++)
                Physics2D.IgnoreCollision(collider2Ds[i], elementsController.collider2Ds[x], false);
        }
    }
}
