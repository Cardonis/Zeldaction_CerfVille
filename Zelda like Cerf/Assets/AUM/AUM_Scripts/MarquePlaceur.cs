﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarquePlaceur : MonoBehaviour
{
    public GameObject mark;
    Player_Main_Controller player;
    BaseAttackCollision baseAttack;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
        baseAttack = GetComponent<BaseAttackCollision>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();


        if (ec != null)
        {

            if ((ec.projected == false || baseAttack.player.canSpringAttack) && ec.GetComponentInChildren<MarquageController>() == null)
            {
                MarquageController newMark = Instantiate(mark, ec.transform).GetComponent<MarquageController>();
                newMark.player = player;

                if(baseAttack.player.canSpringAttack)
                {
                    newMark.venom = true;
                }

                player.marquageManager.ResetTimer(player.marquageDuration);
            }

        }
    }
}
