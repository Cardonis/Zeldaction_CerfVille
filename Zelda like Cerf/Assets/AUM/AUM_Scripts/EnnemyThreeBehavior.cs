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
    public Collider2D physicCollider;
    public float attackForce;

    Vector2 directionForAttack;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();

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
            Wandering();
        }
        else
        {
            if (!attacking)
            {
                speed = attackSpeed;

                if (Vector2.Distance(transform.position, player.transform.position) <= 9.5f)
                {
                    directionForAttack = (transform.position - player.transform.position).normalized * (1f + 9.5f - Vector2.Distance(transform.position, player.transform.position) );
                }
                else if (Vector2.Distance(transform.position, player.transform.position) >= 10f)
                {
                    directionForAttack = (player.transform.position - transform.position).normalized;
                }
                else
                {
                    if (attackCooldownTimer < attackCooldown)
                    {
                        attackCooldownTimer += Time.deltaTime;
                    }

                    directionForAttack = Vector2.zero;
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
            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }
    }

    public override IEnumerator Attack1()
    {


        yield break;
    }

    public IEnumerator ApplyForce(Player_Main_Controller player ,float forceValue, float levelMultiplicator)
    {
        stuned = true;

        player.projected = true;

        //player.StartCoroutine(player.StunnedFor(1f));

        //player.rb.velocity = new Vector2(0, 0);

        player.rb.velocity = new Vector2(0, 0);

        levelProjected = levelMultiplicator;

        Vector2 direction = player.transform.position - transform.position;

        //player.direction = -direction;

        Collider2D[] ennemyColliders = GetComponentsInChildren<Collider2D>();

        StartCoroutine(DontCollideWithPlayerFor(1f));

        while (direction.magnitude > 0.7f)
        {
            for (int i = 0; i < ennemyColliders.Length - 1; i++)
            {
                Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), true);
            }

            direction = (player.transform.position - transform.position);
            rb.velocity = direction.normalized * (forceValue * Mathf.Sqrt(levelProjected) / 2);
            yield return null;
        }

        player.stunned = false;

        if (GetComponentInChildren<Bullet_Versatil_Controller>() != null)
            Destroy(GetComponentInChildren<Bullet_Versatil_Controller>().gameObject);

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction.normalized * forceValue * levelProjected, ForceMode2D.Impulse);

        for (float i = 2.5f; i > 0; i -= Time.fixedDeltaTime)
        {
            yield return null;
        }

        for (int i = 0; i < ennemyColliders.Length - 1; i++)
        {
            Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), false);
        }

    }
}
