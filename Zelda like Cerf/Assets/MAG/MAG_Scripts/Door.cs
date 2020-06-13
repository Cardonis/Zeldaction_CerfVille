using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Buttonmanager> normalyConnectedButtons = new List<Buttonmanager>();
    [HideInInspector] public bool isOpen;
    Collider2D doorCollider;
    SpriteRenderer doorSprite;
    public TimeConnectors timeConnectors;
    public OrderConnectors orderConnectors;

    public bool roomCleared = false;

    bool wasClosed;
    bool isClosed;
    private bool isPlaying;

    AudioManager audiomanager;
    Animator animator;

    void Start()
    {
        doorCollider = GetComponentInChildren<Collider2D>();
        doorSprite = GetComponentInChildren<SpriteRenderer>();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        animator = GetComponentInChildren<Animator>();
        isPlaying = false;
        StartCoroutine(PlayOneOne());
    }

    void Update()
    {
        isOpen = true;

        if(roomCleared == false)
        {
            foreach (Buttonmanager button in normalyConnectedButtons)
            {
                if (button.isPressed == false)
                {
                    isOpen = false;
                }
            }

            if (timeConnectors != null)
                if (timeConnectors.openDoor == false)
                {
                    isOpen = false;
                }

            if (orderConnectors != null)
                if (orderConnectors.openDoor == false)
                {
                    isOpen = false;
                }
        }

        wasClosed = isClosed;

        animator.SetBool("isOpen", isOpen);
        doorCollider.enabled = !isOpen;

        isClosed = doorCollider.enabled;

        if (wasClosed == true && isClosed == false && isPlaying == false)
        {
            StartCoroutine(audiomanager.PlayOne("DoorOpen", gameObject));
            StartCoroutine(PlayOneOne());
        }




    }

    public IEnumerator PlayOneOne()
    {
        isPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        isPlaying = false;
    }


}
