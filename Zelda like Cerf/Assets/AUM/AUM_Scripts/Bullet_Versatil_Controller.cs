using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Versatil_Controller : MonoBehaviour
{
    public LineRenderer lr;
    [HideInInspector] public Player_Main_Controller player;
    public Rigidbody2D rb;
    public Collider2D col;

    float directionAngle;
    
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
        directionAngle = Vector2.Angle(-rb.velocity, transform.right);

        if(-rb.velocity.y < 0)
        {
            directionAngle = -directionAngle;
        }

        col.transform.rotation = Quaternion.Euler(0, 0, directionAngle);

        lr.SetPosition(0, player.transform.position);
        lr.SetPosition(1, transform.position);

        if (touchedTarget != null)
        {
            transform.position = touchedTarget.position;
        }

        if (Vector2.Distance(transform.position, player.transform.position) > maxDistance && touchedTarget == null)
        {
            if (player.marquageManager.marquageControllers.Count != 0)
            {
                player.pressX.SetActive(true);
            }

            player.stunned = false;
            audiomanager.Stop("Capa_Liane");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();

        if(collision.tag == "Wall" || collision.tag == "RoomLimit")
        {
            player.stunned = false;
            AudioManager.instance.Stop("Capa_Liane");
            Destroy(gameObject);
            return;
        }

        if (ec != null && ec.GetComponentInChildren<Bullet_Versatil_Controller>() == null)
        {
            if (ec.GetComponentInChildren<BulletAttack1>() != null || ec.GetComponentInChildren<Bullet_Versatil_Controller>() != null)
            {
                player.stunned = false;
                AudioManager.instance.Stop("Capa_Liane");
                Destroy(gameObject);
                return;
            }

            rb.velocity = new Vector2(0, 0);
            AudioManager.instance.Play("Capa_Grab");
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
                if(bonusLevelProjecting != 0)
                {
                    if (player.cameraShake.shaking == false)
                        player.cameraShake.lastCameraShake = player.cameraShake.StartCoroutine(player.cameraShake.CameraShakeFor(0.1f, 0.1f, 0.25f));

                    ec.trailParticleSystem.loop = true;
                    ec.trailParticleSystem.Play();
                }

                ec.StartTakeForce(player.GetComponent<Player_Main_Controller>().forceValueVersatilAttack, levelProjecting + bonusLevelProjecting);
            }
            
        }
    }
}
