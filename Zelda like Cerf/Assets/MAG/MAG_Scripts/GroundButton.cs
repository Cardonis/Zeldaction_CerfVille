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
    Animator animatorNotFullyPressed;

    Player_Main_Controller player;

    AudioManager audiomanager;
    bool wasPressed;
    bool IsPlaying;
    bool notFullyPressed;

    void Start()
    {
        animatorNotFullyPressed = GetComponentInChildren<Animator>();
        animator = GetComponentInChildren<Animator>();
        pierreFilter.useTriggers = true;
        isPressed = false;
        buttonsprite = GetComponentInChildren<SpriteRenderer>();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        IsPlaying = false;
        StartCoroutine(PlayOneOne());
        notFullyPressed = false;
    }

    void Update()
    {
        animatorNotFullyPressed.SetBool("notFullyPressed", notFullyPressed);
        wasPressed = isPressed;
        animator.SetBool("isPressed", isPressed);
        Physics2D.OverlapBox(transform.position, buttonSize, 0, pierreFilter, pierreColliders);
        currentPressionMass = 0;
        for (int i=0; i<pierreColliders.Count; i++)
        {
            Caisse_Controller element = pierreColliders[i].GetComponentInParent<Caisse_Controller>();
            if(element != null)
            {
                currentPressionMass += element.mass;
                player = element.player;
            }
           
        }
        if (currentPressionMass >= totalMass)
        {
            notFullyPressed = false;
            isPressed = true;
            if(wasPressed == false)
            {
                if (player.cameraShake.shaking == false)
                    player.cameraShake.lastCameraShake = player.cameraShake.StartCoroutine(player.cameraShake.CameraShakeFor(0.1f, 0.1f, 0.25f));

                if (lastStarting != null)
                    StopCoroutine(lastStarting);

                lastStarting = StartCoroutine(StartLineLit(dissolveMaterial.material.GetFloat("_Fade"), 1f, 0.5f));
            }

            if (wasPressed == false && IsPlaying == false) {StartCoroutine(audiomanager.PlayOne("ButtonOn", gameObject)); StartCoroutine(PlayOneOne()); }
        }
        else if(currentPressionMass <= totalMass && currentPressionMass != 0)
        {
            isPressed = false;

            if (notFullyPressed == false)
            {
                if (lastStarting != null)
                    StopCoroutine(lastStarting);

                lastStarting = StartCoroutine(StartLineLit(dissolveMaterial.material.GetFloat("_Fade"), 0.5f, 0.5f));
            }

            notFullyPressed = true;
        }
        else
        {
            isPressed = false;

            if (wasPressed == true || notFullyPressed == true)
            {
                if (lastStarting != null)
                    StopCoroutine(lastStarting);

                lastStarting = StartCoroutine(StartLineLit(dissolveMaterial.material.GetFloat("_Fade"), 0f, 0.5f));
            }

            if ((wasPressed == true || notFullyPressed == true)&& IsPlaying == false) { StartCoroutine(audiomanager.PlayOne("ButtonOn", gameObject)); StartCoroutine(PlayOneOne()); }

            notFullyPressed = false;
        }
        
    }

    public IEnumerator PlayOneOne()
    {
        IsPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        IsPlaying = false;
    }
}
