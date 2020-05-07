using System.Collections;
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

        if(ec != null)
        {
            ec.rb.constraints = RigidbodyConstraints2D.None;
            ec.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (player.part != 1)
        {

            Ennemy_Controller enC = collision.GetComponent<Ennemy_Controller>();

            if (enC != null )
                if (enC.attacking == true)
                {
                    enC.projected = false;
                }

            if (ec != null )
            {
                
                if ((ec.projected == false || baseAttack.player.canSpringAttack) && ec.GetComponentInChildren<MarquageController>() == null)
                {
                    MarquageController newMark = Instantiate(mark, ec.transform).GetComponent<MarquageController>();
                    newMark.player = player;
                    FindObjectOfType<AudioManager>().Play("Coup_de_vent_sweetSpot");

                    if (baseAttack.player.canSpringAttack)
                    {
                        newMark.venom = true;
                    }

                }
            }
            
        }
    }
}
