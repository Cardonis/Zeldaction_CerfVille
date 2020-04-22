using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionController : MonoBehaviour
{
    [HideInInspector] public RoomController activeRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Transition(RoomController target, Player_Main_Controller player)
    {
        player.stunned = true;

        if (activeRoom != null)
        {
            activeRoom.active = false;
        }

        while (!Mathf.Approximately(transform.position.x, target.transform.position.x) || !Mathf.Approximately(transform.position.y, target.transform.position.y))
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0.15f);
            player.rb.velocity = (target.transform.position - transform.position).normalized * 40f * Time.fixedDeltaTime;


            yield return null;
        }

        if(activeRoom != null)
        {
            activeRoom.GetComponent<Collider2D>().enabled = true;
            for (int i = 0; i < activeRoom.limitColliders.Count; i++)
            {
                activeRoom.limitColliders[i].enabled = false;
            }
        }

        activeRoom = target;
        activeRoom.active = true;

        activeRoom.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < activeRoom.limitColliders.Count; i++)
        {
            activeRoom.limitColliders[i].enabled = true;
        }
        //A remplacer
        for (int i = 0; i < activeRoom.ennemies.Count; i++)
        {
            activeRoom.ennemies[i].enabled = true;
        }

        player.stunned = false;
    }

}
