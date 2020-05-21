﻿using UnityEngine;

public class BaseAttackCollision : MonoBehaviour
{
    [HideInInspector] public Player_Main_Controller player;
    [HideInInspector] public Bullet_Versatil_Controller bulletController;
    public float forceValue;
    public float levelMultiplicator;
    float bulletLevel = 1;
    float forceBulletLevel = 1;

    public ParticleSystem sweetspotParticleSystem;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        Elements_Controller ec = collider.GetComponentInParent<Elements_Controller>();

        if(ec != null)
        {

            for (int i = 0; i < ec.collider2Ds.Count; i++)
            {
                ec.collider2Ds[i].gameObject.layer = ec.ennemyCollidersLayers[i];
            }

            Ennemy_Controller enC = ec.GetComponent<Ennemy_Controller>();

            if(enC != null)
            {
                if (enC.attacking == true)
                {
                    enC.StopCoroutine(enC.lastAttack);
                    enC.attacking = false;
                    

                    EnnemiOneBehavior eob = enC.GetComponent<EnnemiOneBehavior>();
                    BossBehavior bb = enC.GetComponent<BossBehavior>();

                    if(eob != null)
                    {
                        eob.attackCollider.enabled = false;
                        eob.projected = false;
                    }

                    if(bb != null)
                    {
                        bb.dashAttackCollider.enabled = false;
                        bb.projected = false;
                    }
                }
            }
            else
            {
                ec.rb.constraints = RigidbodyConstraints2D.None;
                ec.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            if (ec.playerProjected == false || player.canSpringAttack == true)
            {
                bulletController = ec.GetComponentInChildren<Bullet_Versatil_Controller>();

                if (GetComponent<MarquePlaceur>() != null)
                {
                    sweetspotParticleSystem.Play();
                }

                    if (bulletController != null)
                {
                    ec.StopTakeForce();
                    ec.projected = false;
                    if (GetComponent<MarquePlaceur>() != null)
                    {
                        sweetspotParticleSystem.Play();

                        bulletLevel = bulletController.levelProjecting * 4;

                        forceBulletLevel = 2;
                    }

                    Destroy(bulletController.gameObject);
                }

                player.canSpringAttack = false;
                ec.TakeForce(player.directionAim.normalized, forceValue / forceBulletLevel, levelMultiplicator * (bulletLevel));
            }

            bulletLevel = 1;
            forceBulletLevel = 1;
        }


    }
}
