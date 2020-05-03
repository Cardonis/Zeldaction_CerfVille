using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : Buttonmanager
{
    public int totalMass;
    public Vector2 buttonSize;
    List<Collider2D> pierreColliders = new List<Collider2D>();
    ContactFilter2D pierreFilter = new ContactFilter2D();
    int currentPressionMass;
    SpriteRenderer buttonsprite;
    Animator animator;

    AudioManager audiomanager;
    bool wasPressed;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        pierreFilter.useTriggers = true;
        isPressed = false;
        buttonsprite = GetComponentInChildren<SpriteRenderer>();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        wasPressed = isPressed;
        animator.SetBool("isPressed", isPressed);
        Physics2D.OverlapBox(transform.position, buttonSize, 0, pierreFilter, pierreColliders);
        currentPressionMass = 0;
        for(int i=0; i<pierreColliders.Count; i++)
        {
            Elements_Controller element = pierreColliders[i].GetComponentInParent<Elements_Controller>();
            if(element != null)
            {
                currentPressionMass += pierreColliders[i].GetComponentInParent<Elements_Controller>().mass;
            }
           
        }
        if (currentPressionMass >= totalMass)
        {
            isPressed = true;
            if (wasPressed == false) {StartCoroutine(audiomanager.PlayOne("ButtonOn", gameObject));}
        }
        else
        {
            isPressed = false;
        }
    }
}
