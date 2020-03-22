using UnityEngine;

public class BaseAttackCollision : MonoBehaviour
{
    Player_Main_Controller player;
    public float forceValue;
    public float levelMultiplicator;
    float bulletLevel;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        Elements_Controller ec = collider.GetComponentInParent<Elements_Controller>();

        if(ec != null)
        {
            Bullet_Versatil_Controller bC = ec.GetComponentInChildren<Bullet_Versatil_Controller>();

            if (bC != null)
            {
                ec.StopTakeForce();
                ec.projected = false;
                if(GetComponent<MarquePlaceur>() != null)
                {
                    bulletLevel = bC.levelProjecting;
                }
                
                Destroy(bC.gameObject);
            }

            if (ec.projected == false)
                ec.TakeForce(player.direction.normalized, forceValue, levelMultiplicator * bulletLevel);

            bulletLevel = 1;
        }


    }
}
