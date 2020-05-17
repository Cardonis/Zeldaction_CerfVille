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
    bool IsPlaying;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        pierreFilter.useTriggers = true;
        isPressed = false;
        buttonsprite = GetComponentInChildren<SpriteRenderer>();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        IsPlaying = false;
        StartCoroutine(PlayOneOne());
    }

    void Update()
    {
        wasPressed = isPressed;
        animator.SetBool("isPressed", isPressed);
        Physics2D.OverlapBox(transform.position, buttonSize, 0, pierreFilter, pierreColliders);
        currentPressionMass = 0;
        for(int i=0; i<pierreColliders.Count; i++)
        {
            Caisse_Controller element = pierreColliders[i].GetComponentInParent<Caisse_Controller>();
            if(element != null)
            {
                currentPressionMass += pierreColliders[i].GetComponentInParent<Caisse_Controller>().mass;
            }
           
        }
        if (currentPressionMass >= totalMass)
        {
            isPressed = true;
            if (wasPressed == false && IsPlaying == false) {StartCoroutine(audiomanager.PlayOne("ButtonOn", gameObject)); StartCoroutine(PlayOneOne()); }
        }
        else
        {
            isPressed = false;
        }
    }

    public IEnumerator PlayOneOne()
    {
        IsPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        IsPlaying = false;
    }
}
