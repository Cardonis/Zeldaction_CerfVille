using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public Animator animator;
    public Collider2D col;
    bool isOpen;
    public Door theDoor;
    public bool isInTheRoom;
    public float timerClose;
    public bool hasExitRoom;

    void Start()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        isInTheRoom = false;
        hasExitRoom = false;
        timerClose = 0f;
    }

    private void Update()
    {
        if (hasExitRoom)
        {
            timerClose += Time.deltaTime;
            if (timerClose > 3f)
            {
                isOpen = true;
                col.enabled = false;
                animator.SetBool("isOpen", isOpen);
                isInTheRoom = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Main_Controller player = collision.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null && isInTheRoom == false)
        {
            timerClose = 0f;
            hasExitRoom = false;
            StartCoroutine(DoorClose());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player_Main_Controller player = collision.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null && isInTheRoom)
        {
            hasExitRoom = true;
        }
    }

    IEnumerator DoorClose()
    {
        yield return new WaitForSeconds(1f);
        isOpen = false;
        col.enabled = true;
        animator.SetBool("isOpen", isOpen);
        isInTheRoom = true;
    }
}
