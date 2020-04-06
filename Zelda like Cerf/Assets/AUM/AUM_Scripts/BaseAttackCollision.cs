using UnityEngine;

public class BaseAttackCollision : MonoBehaviour
{
    [HideInInspector] public Player_Main_Controller player;
    [HideInInspector] public Bullet_Versatil_Controller bulletController;
    public float forceValue;
    public float levelMultiplicator;
    float bulletLevel = 1;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        Elements_Controller ec = collider.GetComponentInParent<Elements_Controller>();

        if(ec != null)
        {
            bulletController = ec.GetComponentInChildren<Bullet_Versatil_Controller>();

            if (bulletController != null)
            {
                ec.StopTakeForce();
                ec.projected = false;
                if(GetComponent<MarquePlaceur>() != null)
                {
                    bulletLevel = bulletController.levelProjecting;
                }
                
                Destroy(bulletController.gameObject);
            }

            Ennemy_Controller enC = ec.GetComponent<Ennemy_Controller>();

            if(enC != null)
            {
                if(enC.attacking == true)
                {
                    enC.StopCoroutine(enC.lastAttack);
                    enC.attacking = false;
                    

                    EnnemiOneBehavior eob = enC.GetComponent<EnnemiOneBehavior>();

                    if(eob != null)
                    {
                        eob.attackCollider.enabled = false;
                        eob.projected = false;
                    }
                }
            }

            if (ec.projected == false)
            ec.TakeForce(player.directionAim.normalized, forceValue, levelMultiplicator * bulletLevel);

            bulletLevel = 1;
        }


    }
}
