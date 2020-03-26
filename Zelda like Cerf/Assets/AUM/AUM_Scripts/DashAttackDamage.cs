using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackDamage : MonoBehaviour
{
    EnnemiOneBehavior ennemiOneBehavior;
    public float forceProjecting;

    // Start is called before the first frame update
    void Start()
    {
        ennemiOneBehavior = GetComponentInParent<EnnemiOneBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody != null)
            if (collision.attachedRigidbody.transform.tag == "Player" && ennemiOneBehavior.player.projected == false)
            {
                GameObject[] bvcs = GameObject.FindGameObjectsWithTag("PlayerBullet");

                foreach(GameObject bvc in bvcs)
                {
                    Elements_Controller ec = bvc.GetComponentInParent<Elements_Controller>();
                    if (ec != null)
                    {
                        ec.StopCoroutine(ec.lastTakeForce);
                    }

                    Destroy(bvc.gameObject);
                }

                if(ennemiOneBehavior.player.baseAttacking)
                {
                    ennemiOneBehavior.player.StopCoroutine(ennemiOneBehavior.player.lastBaseAttack);
                    ennemiOneBehavior.player.baseAttacking = false;

                    for (int i = 0; i < ennemiOneBehavior.player.baseAttackSRs.Length; i++)
                    {
                        ennemiOneBehavior.player.baseAttackSRs[i].enabled = false;
                    }

                    for (int i = 0; i < ennemiOneBehavior.player.baseAttackColliders.Length; i++)
                    {
                        ennemiOneBehavior.player.baseAttackColliders[i].enabled = false;
                    }
                }

                ennemiOneBehavior.player.TakeForce(ennemiOneBehavior.attackDirection, forceProjecting);

                ennemiOneBehavior.player.StartCoroutine(ennemiOneBehavior.player.TakeDamage(1));
            }
    }
}
