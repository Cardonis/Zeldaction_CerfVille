using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public int trapDamageForPlayer;
    public float stunTimeForPlayer;

    public int trapDamageForEnnemi;
    public float stunTimeForEnnemi;

    bool isActive;
    public float desactivationTime;

    public SpriteRenderer trapSprite;
    bool inCooldown;
   

    void Start()
    {
        isActive = true;
    }
    void Update()
    {
        if ( isActive == false && inCooldown == false)
        {
            Invoke("SetTrapActive", desactivationTime);
            inCooldown = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (inCooldown == false)
        {
            Player_Main_Controller elementplayer = collider.GetComponentInParent<Player_Main_Controller>();
            if (elementplayer != null && isActive == true) 
            {
                elementplayer.StartCoroutine(elementplayer.TakeDamage(trapDamageForPlayer));
                elementplayer.StartCoroutine(elementplayer.StunnedFor(stunTimeForPlayer));
                elementplayer.rb.velocity = Vector2.zero;
                isActive = false;
            }

            Ennemy_Controller elementennemi = collider.GetComponentInParent<Ennemy_Controller>();

            if (elementennemi != null && isActive == true)
            {
                elementennemi.StartTakeDamage(trapDamageForEnnemi);
                StartCoroutine(elementennemi.StunedForSeconds(stunTimeForEnnemi));
                isActive = false;
            }
        }
    }
    private void SetTrapActive()
    {
        isActive = true;
        inCooldown = false;
    }
}
