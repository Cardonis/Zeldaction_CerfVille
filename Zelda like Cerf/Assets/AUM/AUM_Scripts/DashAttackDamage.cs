using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackDamage : MonoBehaviour
{
    Ennemy_Controller ennemyBehavior;
    public float forceProjecting;

    // Start is called before the first frame update
    void Start()
    {
        ennemyBehavior = GetComponentInParent<Ennemy_Controller>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody != null)
            if (collision.attachedRigidbody.transform.tag == "Player" && ennemyBehavior.player.projected == false)
            {
                GameObject[] bvcs = GameObject.FindGameObjectsWithTag("PlayerBullet");

                foreach(GameObject bvc in bvcs)
                {
                    Elements_Controller ec = bvc.GetComponentInParent<Elements_Controller>();
                    if (ec != null)
                    {
                        ec.StopTakeForce();
                    }

                    Destroy(bvc.gameObject);
                }

                if(ennemyBehavior.player.baseAttacking)
                {
                    ennemyBehavior.player.StopCoroutine(ennemyBehavior.player.lastBaseAttack);
                    ennemyBehavior.player.baseAttacking = false;

                    for (int i = 0; i < ennemyBehavior.player.baseAttackSRs.Length; i++)
                    {
                        ennemyBehavior.player.baseAttackSRs[i].SetActive(true);
                    }

                    for (int i = 0; i < ennemyBehavior.player.baseAttackColliders.Length; i++)
                    {
                        ennemyBehavior.player.baseAttackColliders[i].enabled = false;
                    }
                }

                EnnemiOneBehavior eob = ennemyBehavior.GetComponent<EnnemiOneBehavior>();
                if (eob != null)
                    ennemyBehavior.player.TakeForce(eob.attackDirection, forceProjecting);
                else
                {
                    ennemyBehavior.player.TakeForce(ennemyBehavior.direction, forceProjecting);
                }

                if (ennemyBehavior.player.invincible == false)
                    ennemyBehavior.player.StartCoroutine(ennemyBehavior.player.TakeDamage(1));
            }
    }
}
