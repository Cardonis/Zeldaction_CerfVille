using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : Buttonmanager
{
    public int scoreNeeded;
    int detectedObjectMass;
    float detectedObjectProjectionLevel;
    SpriteRenderer buttonSprite;
    Animator animator;

    AudioManager audiomanager;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        isPressed = false;
        buttonSprite = GetComponentInChildren<SpriteRenderer>();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        animator.SetBool("isPressed", isPressed);
        if (isPressed == false)
        {
            detectedObjectMass = 0;
            detectedObjectProjectionLevel = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller element = collision.collider.GetComponentInParent<Elements_Controller>();

        if(element != null)
        {
            detectedObjectMass = element.mass;
            detectedObjectProjectionLevel = element.levelProjected;
        }

        if (detectedObjectMass * detectedObjectProjectionLevel >= scoreNeeded)
        {
            isPressed = true;
            StartCoroutine(audiomanager.PlayOne("ButtonOn", gameObject));
        }
    }
}
