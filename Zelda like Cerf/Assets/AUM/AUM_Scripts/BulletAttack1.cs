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
            ennemyController.GetComponent<EnnemyThreeBehavior>().stuned = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Main_Controller pmc = collision.attachedRigidbody.GetComponent<Player_Main_Controller>();
        Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();

        if (collision.tag == "Wall")
        {
            ennemyController.GetComponent<EnnemyThreeBehavior>().stuned = false;
            Destroy(gameObject);
        }

        if (ec != null || pmc != null)
        {
            rb.velocity = new Vector2(0, 0);

            touchedTarget = collision.transform;

            ec.stuned = false;

            transform.SetParent(ec.transform);

            if(pmc != null)
            {
                pmc.physicCollider.enabled = false;
            }
            else
            GetComponentInChildren<Collider2D>().enabled = false;

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
                ennemyController.GetComponent<EnnemyThreeBehavior>().stuned = false;
                Destroy(gameObject);
            }
            else
            {
                ec.StartTakeForce(ennemyController.GetComponent<Player_Main_Controller>().forceValueVersatilAttack, levelProjecting);
            }

        }
        else if(pmc != null)
        {

        }
    }
}
