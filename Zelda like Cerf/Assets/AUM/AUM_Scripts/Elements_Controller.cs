using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements_Controller : MonoBehaviour
{
    [HideInInspector] public bool projected = false;
    [HideInInspector] public bool playerProjected = false;
    [HideInInspector] public bool stuned = false;
    [HideInInspector] public Rigidbody2D rb;

    [Range(0, 3)] public int mass;

    [HideInInspector] public float levelProjected = 0;

    [HideInInspector] public Player_Main_Controller player;
    [HideInInspector] public MarquageManager marquageManager;

    public float recoveryValue;

    [HideInInspector] public Coroutine lastTakeForce;

    [HideInInspector] public Vector2 initialPosition;

    [HideInInspector] public bool spawned = false;

    AudioManager audiomanager;


    public void TakeForce(Vector2 direction, float forceValue, float levelMultiplicator)
    {
        projected = true;

        playerProjected = true;

        rb.velocity = new Vector2(0, 0);

        levelProjected = levelMultiplicator;

        rb.AddForce(direction * forceValue * levelMultiplicator, ForceMode2D.Impulse);

    }

    public void StartTakeForce(float forceValue, float levelMultiplicator)
    {
         lastTakeForce = StartCoroutine(TakeForce(forceValue, levelMultiplicator));
    }

    public void StopTakeForce()
    {
        StopCoroutine(lastTakeForce);
    }

    public IEnumerator TakeForce(float forceValue, float levelMultiplicator)
    {
        projected = true;
        audiomanager.Play("Capa_Liane");
        playerProjected = true;

        //player.StartCoroutine(player.StunnedFor(1f));

        player.StartCanSpringAttack(1f);

        //player.rb.velocity = new Vector2(0, 0);

        rb.velocity = new Vector2(0, 0);

        levelProjected = levelMultiplicator;

        Vector2 direction = player.transform.position - transform.position;

        //player.direction = -direction;

        Collider2D[] ennemyColliders = GetComponentsInChildren<Collider2D>();

        StartCoroutine(DontCollideWithPlayerFor(1f));

        while(direction.magnitude > 0.7f)
        {
            for(int i = 0; i < ennemyColliders.Length - 1; i++)
            {
                Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), true);
            }
            
            direction = (player.transform.position - transform.position );
            rb.velocity = direction.normalized * (forceValue * Mathf.Sqrt(levelProjected) / 2);
            yield return null;
        }
        audiomanager.Stop("Capa_Liane");
        audiomanager.Play("Capa_Lance");
        player.stunned = false;

        if(GetComponentInChildren<Bullet_Versatil_Controller>() != null)
        Destroy(GetComponentInChildren<Bullet_Versatil_Controller>().gameObject);

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction.normalized * forceValue * levelProjected, ForceMode2D.Impulse);

        player.timerCooldownVersatilAttack = 0;
        player.lastAttackVersatilTimer = 0;

        for(float i = 2.5f; i > 0; i -= Time.fixedDeltaTime)
        {
            yield return null;
        }

        for (int i = 0; i < ennemyColliders.Length - 1; i++)
        {
            Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), false);
        }

    }


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();

        marquageManager = GameObject.Find("MarquageManager").GetComponent<MarquageManager>();

        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        initialPosition = transform.position;
    }

    public virtual void FixedUpdate()
    {
        if(stuned == true)
        {
            rb.velocity = new Vector2(0,0);
        }

        if(projected == true && rb.velocity.magnitude < recoveryValue)
        {
            projected = false;
            playerProjected = false;
            rb.velocity = new Vector2(0, 0);
            levelProjected = 0;
        }

    }

    public IEnumerator DontCollideWithPlayerFor(float time)
    {
        Collider2D[] ennemyColliders = GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < ennemyColliders.Length - 1; i++)
        {
            Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), true);
        }

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < ennemyColliders.Length - 1; i++)
        {
            Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), false);
        }
    }

    public IEnumerator StunedForSeconds(float time)
    {
        stuned = true;
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        stuned = false;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller ec = collision.transform.GetComponentInParent<Elements_Controller>();

        if (ec != null)
        {
            MarquageController mc = GetComponentInChildren<MarquageController>();

            if(mc != null)
            if (mc.venom == true)
            {
                MarquageController newMark = Instantiate(mc.gameObject, ec.transform).GetComponent<MarquageController>();
                newMark.player = player;

                newMark.venom = true;

                player.marquageManager.ResetTimer(player.marquageDuration);
            }
        }
    }

    
}
