using UnityEngine;

public class BaseAttackCollision : MonoBehaviour
{
    Player_Main_Controller player;
    public float forceValue;
    public float levelMultiplicator;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        Elements_Controller ec = collider.GetComponentInParent<Elements_Controller>();

        if(ec != null)
        {
            if (ec.projected == false)
                ec.TakeForce(player.direction.normalized, forceValue, levelMultiplicator);
        }

    }
}
