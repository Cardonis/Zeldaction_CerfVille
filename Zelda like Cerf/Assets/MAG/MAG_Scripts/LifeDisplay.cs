using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    int currentHealth;
    int numberOfHearts;

    public Color hearthRegenColor;

    public Image[] hearths;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite halfHeart;
    public Player_Main_Controller player;
    void Start()
    {
        currentHealth = player.currentLife;
        numberOfHearts = player.maxLife / 2;
    }

    void Update()
    {
        currentHealth = player.currentLife;
        numberOfHearts = player.maxLife / 2;
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
            else
            {
                hearths[i].gameObject.SetActive(true);
            }

            hearths[i].color = Color.white;

            i++;
        }

        if (player.gainingLife == true)
        {
            int hearthFilling = (int)Mathf.Ceil(currentHealth / 2f);
            hearths[hearthFilling - 1].color = hearthRegenColor;
        }
    }
}
