using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MarquageManager : MonoBehaviour
{
    public List<MarquageController> marquageControllers;

    public Player_Main_Controller player;

    public void Update()
    {
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
