using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlower : Elements_Controller
{
    public float DestroyDistance;
    public int healValue;
    
    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public IEnumerator FlowerGrabed(Player_Main_Controller player, Vector2 playerTransform)
    {
        
        
        while (gameObject != null)
        {
            if (Vector2.Distance((Vector2)transform.position, playerTransform) < DestroyDistance)
            {
                if(player.currentLife <= player.maxLife)
                {
                    player.currentLife += healValue;
                }

                Destroy(gameObject);
                
            }
            yield return new WaitForFixedUpdate();
        }

    }
    public IEnumerator FlowerGrabedBaseAttack(Player_Main_Controller player, Vector2 playerTransform)
    {
        bool isFinished = false;
        Vector2 direction = playerTransform - (Vector2)transform.position;

        while (isFinished == false)
        {
            rb.velocity = direction * 2f;
            if (Vector2.Distance((Vector2)transform.position, playerTransform) < DestroyDistance)
            {
                player.currentLife += healValue;
                Destroy(gameObject);
                isFinished = true;

            }
            yield return new WaitForFixedUpdate();
        }

    }
}
