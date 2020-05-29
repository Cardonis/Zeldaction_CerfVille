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
    bool IsPlaying;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        isPressed = false;
        buttonSprite = GetComponentInChildren<SpriteRenderer>();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        IsPlaying = false;
        StartCoroutine(PlayOneOne());
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
            if (element.projected == true)
            {
                detectedObjectMass = element.mass;
                detectedObjectProjectionLevel = element.levelProjected;

                if (detectedObjectMass * detectedObjectProjectionLevel >= scoreNeeded)
                {
                    if (lastStarting != null)
                        StopCoroutine(lastStarting);

                    lastStarting = StartCoroutine(StartLineLit(0f, 1f, 0.5f));

                    if (element.player.cameraShake.shaking == false)
                        element.player.cameraShake.lastCameraShake = element.player.cameraShake.StartCoroutine(element.player.cameraShake.CameraShakeFor(0.1f, 0.1f, 0.25f));
                    isPressed = true;
                    if (IsPlaying == false) { StartCoroutine(audiomanager.PlayOne("ButtonOn", gameObject)); StartCoroutine(PlayOneOne()); }
                }
                else
                {
                    if (lastStarting != null)
                        StopCoroutine(lastStarting);

                    lastStarting = StartCoroutine(StartLineLit(0f, 0.5f, 0.2f));
                    lastStarting = StartCoroutine(StartLineLit(0.5f, 0f, 0.2f));
                }
            }
            
        }
    }

    public IEnumerator PlayOneOne()
    {
        IsPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        IsPlaying = false;
    }
}
