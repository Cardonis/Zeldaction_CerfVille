using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlower : Elements_Controller
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Main_Controller player = collision.GetComponentInParent<Player_Main_Controller>();
        if (player != null)
        {
            player.life++;
            Destroy(gameObject);
        }
    }

}
