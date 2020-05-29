using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsDisplay : MonoBehaviour
{
    public bool istips1;
    public bool istips2;
    public bool istips3;

    public GameObject tips1, tips2, tips3;

    public TipsTrigger tipsTrigger;

    bool asTalked;

    private void Update()
    {
        asTalked = tipsTrigger.asTalked;

        if (asTalked == true)
        {
            if(istips1 == true)
            {
                tips1.SetActive(true);
            }
            if (istips2 == true)
            {
                tips2.SetActive(true);
            }
            if (istips3 == true)
            {
                tips3.SetActive(true);
            }
        }
    }
}
