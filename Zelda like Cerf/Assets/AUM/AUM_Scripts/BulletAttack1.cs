using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack1 : MonoBehaviour
{
    public LineRenderer lr;
    [HideInInspector] public Transform ennemyController;
    public Rigidbody2D rb;
    [HideInInspector] public float maxDistance;

    [HideInInspector] public float levelProjecting;

    Transform touchedTarget;


    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, ennemyController.position);
        lr.SetPosition(1, transform.position);

        if (touchedTarget != null)
        {
            transform.position = touchedTarget.position;
        }

        if (Vector2.Distance(transform.position, ennemyController.position) > maxDistance && touchedTarget == null)
        {
            ennemyController.GetComponent<Ennemy_Controller>().stuned = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Main_Controller pmc = null;
        if (collision.attachedRigidbody != null)
            pmc = collision.attachedRigidbody.GetComponent<Player_Main_Controller>();
        Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();

        if (collision.tag == "Wall" || collision.tag == "RoomLimit")
        {
            ennemyController.GetComponent<Ennemy_Controller>().stuned = false;
            Destroy(gameObject);
            return;
        }

        if (ec != null || pmc != null)
        {
            rb.velocity = new Vector2(0, 0);

            touchedTarget = collision.transform;

            GetComponentInChildren<Collider2D>().enabled = false;

            BossBehavior bb = ennemyController.GetComponent<BossBehavior>();
            EnnemyThreeBehavior etb = ennemyController.GetComponent<EnnemyThreeBehavior>();

            if (pmc != null)
            {
                if (pmc.GetComponentInChildren<BulletAttack1>() != null)
                {
                    ennemyController.GetComponent<Ennemy_Controller>().stuned = false;
                    Destroy(gameObject);
                    return;
                }

                if (etb != null)
                    etb.StartCoroutine(etb.ApplyForce(pmc));
                else if (bb != null)
                    bb.StartCoroutine(bb.ApplyForce(pmc));
            }
            else if (ec != null)
            {
                if (ec.GetComponentInChildren<BulletAttack1>() != null || ec.GetComponentInChildren<Bullet_Versatil_Controller>() != null)
                {
                    ennemyController.GetComponent<Ennemy_Controller>().stuned = false;
                    Destroy(gameObject);
                    return;
                }

                ec.rb.constraints = RigidbodyConstraints2D.None;
                ec.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                Ennemy_Controller enC = ec.GetComponent<Ennemy_Controller>();

                if (enC != null)
                {
                    if (enC.attacking == true)
                    {
                        enC.StopCoroutine(enC.lastAttack);
                        enC.attacking = false;

                        EnnemiOneBehavior eob = enC.GetComponent<EnnemiOneBehavior>();

                        if (eob != null)
                        {
                            eob.attackCollider.enabled = false;
                        }
                    }
                }

                Ronces ronce = ec.GetComponent<Ronces>();
                if (ronce != null)
                {
                    ennemyController.GetComponent<Ennemy_Controller>().stuned = false;
                    ennemyController.GetComponent<Ennemy_Controller>().attacking = false;
                    Destroy(gameObject);
                    return;
                }

                if (etb != null)
                {
                    etb.lastAttack = etb.StartCoroutine(etb.ApplyForce(ec));
                }
                else if (bb != null)
                    bb.StartCoroutine(bb.ApplyForce(ec));
            }

            transform.SetParent(collision.transform);
        }
    }
}
