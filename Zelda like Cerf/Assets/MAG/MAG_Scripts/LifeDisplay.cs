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
    public Sprite halfHeart;
    void Start()
    {
        
    }

    void Update()
    {
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        int life = currentHealth;
        int i = 0;
        while(i < hearths.Length)
        {
            if(life - 2 >= 0)
            {
                hearths[i].sprite = fullHeart;
                life -= 2;
            }
            else if(life == 1)
            {
                hearths[i].sprite = halfHeart;
                life--;
            }
            else if(life == 0)
            {
                hearths[i].sprite = emptyHeart;
            }


            if(i >= numberOfHearts)
            {
                hearths[i].gameObject.SetActive(false);
            }

            i++;
        }
    }
}
