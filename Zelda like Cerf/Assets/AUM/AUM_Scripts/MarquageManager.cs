using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MarquageManager : MonoBehaviour
{
    public List<MarquageController> marquageControllers;

    Player_Main_Controller player;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }

    public void Update()
    {
        for (int i = 0; i < marquageControllers.Count; i++)
        {
            if (marquageControllers[i] == null)
            {
                marquageControllers.RemoveAt(i);
            }
        }

        marquageControllers = marquageControllers.OrderBy(
        x => Vector2.Distance(player.transform.position, x.transform.position)
        ).ToList();
        
    }

    public void ResetTimer(float durationToReset)
    {
        for(int i = 0; i < marquageControllers.Count; i++)
        {
            marquageControllers[i].marquageTimer = durationToReset;
        }
    }

}
