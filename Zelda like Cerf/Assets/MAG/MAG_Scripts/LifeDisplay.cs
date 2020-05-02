using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    public int currentHealth;
    public int numberOfHearts;

    public Image[] hearths;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    void Start()
    {
        
    }

    void Update()
    {
        
        if(currentHealth> numberOfHearts)
        {
            currentHealth = numberOfHearts;
        }

        for (int i = 0; i < numberOfHearts; i++)
        {
            if (i < currentHealth)
            {
                hearths[i].sprite = fullHeart;
            }
            else
            {
                hearths[i].sprite = emptyHeart;
            }

            if (i < numberOfHearts)
            {
                hearths[i].enabled = true;
            }
            else
            {
                hearths[i].enabled = false;
            }
        }
    }
}
