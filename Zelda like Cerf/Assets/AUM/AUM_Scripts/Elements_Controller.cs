﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements_Controller : MonoBehaviour
{
    [HideInInspector] public bool projected = false;
    [HideInInspector] public bool stuned = false;
    [HideInInspector] public Rigidbody2D rb;

    [Range(0, 3)] public int mass;

    [HideInInspector] public float levelProjected = 0;

    [HideInInspector] public Player_Main_Controller player;
    [HideInInspector] public MarquageManager marquageManager;

    public float recoveryValue;

    [HideInInspector] public Coroutine lastTakeForce;


    public void TakeForce(Vector2 direction, float forceValue, float levelMultiplicator)
    {
        projected = true;

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

        player.projected = true;

        player.StartCanSpringAttack(1f);

        player.rb.velocity = new Vector2(0, 0);

        rb.velocity = new Vector2(0, 0);

        levelProjected = levelMultiplicator;

        Vector2 direction = player.transform.position - transform.position;

        Vector2 direction2 = direction;

        player.direction = -direction;

        Collider2D[] ennemyColliders = GetComponentsInChildren<Collider2D>();

        StartCoroutine(DontCollideWithPlayerFor(1.5f));

        while(direction.magnitude > 0.7f)
        {
            for(int i = 0; i < ennemyColliders.Length - 1; i++)
            {
                Physics2D.IgnoreCollision(ennemyColliders[i], player.GetComponentInChildren<Collider2D>(), true);
            }
            
            direction = (player.transform.position - transform.position );
            rb.velocity = direction2.normalized * (forceValue * Mathf.Sqrt(levelProjected) / 2);
            yield return null;
        }

        player.projected = false;

        Destroy(GetComponentInChildren<Bullet_Versatil_Controller>().gameObject);

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction2.normalized * forceValue * levelProjected, ForceMode2D.Impulse);

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
