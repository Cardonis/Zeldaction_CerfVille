using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlower : MonoBehaviour
{
    bool playerIsNear;
    Player_Main_Controller player;

    public int healValue;

    private void Start()
    {
        playerIsNear = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller p = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (p != null)
        {
            player = p;
            if(player.currentLife != player.maxLife)
                player.pressX.SetActive(true);
            playerIsNear = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Player_Main_Controller player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            player.pressX.SetActive(false);
            playerIsNear = false;
        }

    }
    private void Update()
    {
        if (Input.GetButtonDown("X") && playerIsNear == true)
        {
            if (player.currentLife != player.maxLife)
            {
                player.pressX.SetActive(false);
                player.currentLife += healValue;

                if (player.currentLife > player.maxLife)
                {
                    player.currentLife = player.maxLife;
                }

                Destroy(gameObject);
            }
        }
    }
}
