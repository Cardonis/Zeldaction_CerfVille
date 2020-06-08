using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Elements_Controller : MonoBehaviour
{
    [HideInInspector] public bool projected = false;
    [HideInInspector] public bool playerProjected = false;
    [HideInInspector] public bool stuned = false;
    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public bool isTractable = true;

    [Range(0, 3)] public int mass;

    [HideInInspector] public float levelProjected = 0;

    [HideInInspector] public Player_Main_Controller player;
    [HideInInspector] public MarquageManager marquageManager;

    public float recoveryValue;

    [HideInInspector] public Coroutine lastTakeForce;
    [HideInInspector] public Coroutine lastDontCollideWithPlayer;

    [HideInInspector] public Vector2 initialPosition;

    [HideInInspector] public bool spawned = false;

    [HideInInspector] public AudioManager audiomanager;

    public List<Collider2D> collider2Ds;

    [HideInInspector] public List<int> ennemyCollidersLayers = new List<int>();

    [HideInInspector] public Vector3 velocityBeforeImpact;
    [HideInInspector] public float velocityBeforeImpactAngle;

    [HideInInspector] public float initialDrag;

    public OutlineController outlineController;

    public ParticleSystem trailParticleSystem;

    [HideInInspector] public float trailParticleSystemAngle;

    public void TakeForce(Vector2 direction, float forceValue, float levelMultiplicator)
    {
        projected = true;

        stuned = false;

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
        for (int i = 0; i < collider2Ds.Count; i++)
        {
            collider2Ds[i].gameObject.layer = ennemyCollidersLayers[i];
        }

        player.canSpringAttack = false;

        if(lastTakeForce != null)
        StopCoroutine(lastTakeForce);
    }

    public IEnumerator TakeForce(float forceValue, float levelMultiplicator)
    {
        projected = true;
        audiomanager.Play("Capa_Liane");
        playerProjected = true;

        Caisse_Controller cc = GetComponent<Caisse_Controller>();

        if(cc != null)
        {
            cc.rb.mass = cc.initialMass;
        }

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            collider2Ds[i].gameObject.layer = 15;
        }

        player.canSpringAttack = true;

        //player.StartCanSpringAttack(1f);

        rb.velocity = new Vector2(0, 0);

        levelProjected = levelMultiplicator;

        Vector2 direction = player.transform.position - transform.position;

        //StartCoroutine(DontCollideWithPlayerFor(1f));

        while (direction.magnitude > 1.5f)
        {
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

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            collider2Ds[i].gameObject.layer = ennemyCollidersLayers[i];
        }

        player.canSpringAttack = false;

        if (lastDontCollideWithPlayer != null)
            StopCoroutine(lastDontCollideWithPlayer);

         StartCoroutine(DontCollideWithPlayerFor(0.5f));

    }


    public virtual void Start()
    {
        for (int i = 0; i < collider2Ds.Count; i++)
        {
            ennemyCollidersLayers.Add(collider2Ds[i].gameObject.layer);
        }

        velocityBeforeImpact = Vector2.zero;

        isTractable = true;

        rb = GetComponent<Rigidbody2D>();

        initialDrag = rb.drag;

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

        trailParticleSystemAngle = Vector2.Angle(transform.right, -rb.velocity);

        if (-rb.velocity.y < 0)
        {
            trailParticleSystemAngle = -trailParticleSystemAngle;
        }

        if(GetComponent<Ronces>() == null)
        trailParticleSystem.transform.rotation = Quaternion.Euler(0, 0, trailParticleSystemAngle);

        Invoke("Velocity10FramesAgo", 0.05f);

        velocityBeforeImpactAngle = Vector2.Angle(velocityBeforeImpact, transform.right);

        if (velocityBeforeImpact.y < 0)
        {
            velocityBeforeImpactAngle = -velocityBeforeImpactAngle;
        }

        if (projected == true && rb.velocity.magnitude < recoveryValue)
        {
            projected = false;
            playerProjected = false;
            rb.velocity = new Vector2(0, 0);
            levelProjected = 0;

            trailParticleSystem.loop = false;
        }

    }

    public void Velocity10FramesAgo()
    {
        velocityBeforeImpact = rb.velocity;
    }

    public IEnumerator DontCollideWithPlayerFor(float time)
    {
        for (int i = 0; i < collider2Ds.Count; i++)
        {
            Physics2D.IgnoreCollision(collider2Ds[i], player.physicCollider, true);
        }

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            Physics2D.IgnoreCollision(collider2Ds[i], player.GetComponentInChildren<Collider2D>(), false);
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
            if (mc.venom == true && ec.GetComponentInChildren<MarquageController>() == null)
            {
                MarquageController newMark = Instantiate(mc.gameObject, ec.transform).GetComponent<MarquageController>();
                newMark.player = player;

                newMark.venom = true;

                player.marquageManager.ResetTimer(player.marquageDuration);
            }
        }
    }

    
}
