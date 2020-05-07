using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Versatil_Controller : MonoBehaviour
{
    public LineRenderer lr;
    [HideInInspector] public Transform player;
    public Rigidbody2D rb;
    
    [HideInInspector] public float maxDistance;

    [HideInInspector] public float levelProjecting;

    [HideInInspector] public float bonusLevelProjecting;

    Transform touchedTarget;

    AudioManager audiomanager;


    void Start()
    {
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, player.position);
        lr.SetPosition(1, transform.position);

        if (touchedTarget != null)
        {
            transform.position = touchedTarget.position;
        }

        if (Vector2.Distance(transform.position, player.position) > maxDistance && touchedTarget == null)
        {
            player.GetComponent<Player_Main_Controller>().stunned = false;
            audiomanager.Stop("Capa_Liane");
            Destroy(gameObject);           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();

        if(collision.tag == "Wall" || collision.tag == "RoomLimit")
        {
            player.GetComponent<Player_Main_Controller>().stunned = false;
            audiomanager.Stop("Capa_Liane");
            Destroy(gameObject);
        }

        if (ec != null && ec.GetComponentInChildren<Bullet_Versatil_Controller>() == null)
        {
            rb.velocity = new Vector2(0, 0);
            audiomanager.Play("Capa_Grab");
            touchedTarget = collision.transform;

            ec.stuned = false;

            transform.SetParent(ec.transform);

            GetComponentInChildren<Collider2D>().enabled = false;

            Ennemy_Controller enC = ec.GetComponent<Ennemy_Controller>();

            ec.rb.constraints = RigidbodyConstraints2D.None;
            ec.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (enC != null)
            {
                if (enC.attacking == true)
                {
                    enC.StopCoroutine(enC.lastAttack);
                    enC.attacking = false;

                    EnnemiOneBehavior eob = enC.GetComponent<EnnemiOneBehavior>();
                    BossBehavior bb = enC.GetComponent<BossBehavior>();

                    if (eob != null)
                    {
                        eob.attackCollider.enabled = false;
                    }
                    else if (bb != null)
                    {
                        bb.dashAttackCollider.enabled = false;
                    }
                }

            }

            Ronces ronce = ec.GetComponent<Ronces>();
            if(ronce != null)
            {
                if(levelProjecting < (ronce.isARonceEpaisse ? 3 : 1))
                {
                    ronce.StartCantDestroy(gameObject);
                }
                else
                {
                    ronce.StartDestruction(levelProjecting, player.GetComponent<Player_Main_Controller>().forceValueVersatilAttack);
                }
            }
            else
            {
                ec.StartTakeForce(player.GetComponent<Player_Main_Controller>().forceValueVersatilAttack, levelProjecting + bonusLevelProjecting);
            }
            
        }
    }
}
