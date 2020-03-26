using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationCollider : MonoBehaviour
{
    public Player_Main_Controller player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(player.canAccelerate)
        {
            

            if(Input.GetButton("X"))
            {
                Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();
                if (ec != null)
                {
                    switch (ec.levelProjected)
                    {
                        case 0:
                            return;

                        case 1:
                            ec.levelProjected = 2;
                            return;

                        case 2:
                            ec.levelProjected = 4;
                            return;

                        case 4:
                            ec.levelProjected = 4;
                            return;
                    }

                    player.canAccelerate = false;
                }
            }
            
        }
    }
}
