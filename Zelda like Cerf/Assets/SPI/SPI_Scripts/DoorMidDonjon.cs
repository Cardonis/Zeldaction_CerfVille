using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMidDonjon : MonoBehaviour
{
    public AllTimelineController cineEntreBoss;
    Animator animator;
    Collider2D col;
    bool isOpen =false;

    void Start()
    {
        
        col = GetComponentInChildren<Collider2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if(cineEntreBoss.alreadyPlayed == true)
        {
            isOpen = true;
            col.enabled = false;
            animator.SetBool("isOpen", isOpen);
        }
    }
}
