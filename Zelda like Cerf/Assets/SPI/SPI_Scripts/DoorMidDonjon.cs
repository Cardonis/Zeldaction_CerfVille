using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMidDonjon : MonoBehaviour
{
    public AllTimelineController cineEntreBoss;
    Animator animator;
    Collider2D col;
    bool isOpen =false;
    public bool toClose;
    public Door theDoor;

    void Start()
    {
        col = GetComponentInChildren<Collider2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if(cineEntreBoss.wasPlayed == true && toClose == false)
        {
            isOpen = true;
            col.enabled = false;
            animator.SetBool("isOpen", isOpen);
        }
        if (cineEntreBoss.wasPlayed == true && toClose== true)
        {
            theDoor.enabled = false;
            isOpen = false;
            col.enabled = true;
            animator.SetBool("isOpen", isOpen);
        }
    }
}
