using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarquageManager : MonoBehaviour
{
    public List<MarquageController> marquageControllers;

    public void ResetTimer(float durationToReset)
    {
        for(int i = 0; i < marquageControllers.Count; i++)
        {
            marquageControllers[i].marquageTimer = durationToReset;
        }
    }

}
