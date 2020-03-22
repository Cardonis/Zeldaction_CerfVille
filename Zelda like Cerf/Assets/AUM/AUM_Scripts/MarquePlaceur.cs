using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarquePlaceur : MonoBehaviour
{
    public GameObject mark;
    Player_Main_Controller player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();


        if (ec != null)
        {

            if (ec.projected == false && ec.GetComponentInChildren<MarquageController>() == null)
            {
                MarquageController newMark = Instantiate(mark, ec.transform).GetComponent<MarquageController>();
                newMark.player = player;

                player.marquageManager.ResetTimer(player.marquageDuration);
            }

        }
    }
}
