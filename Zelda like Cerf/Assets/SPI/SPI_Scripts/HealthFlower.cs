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
                player.life += healValue;
                Destroy(gameObject);
                
            }
            yield return new WaitForFixedUpdate();
        }

    }
}
