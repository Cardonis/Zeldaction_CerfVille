using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public Animator animator;
    public Collider2D col;
    bool isOpen;
    public Door theDoor;
    public Collider2D detectInRoom;
    public Collider2D detectOut;
    public bool inBossRoom;

    void Start()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        inBossRoom = false;
    }

    private void Update()
    {
        if (inBossRoom == true)
        {
            detectOut.enabled = true;
            detectInRoom.enabled = false;
        }

        if (inBossRoom == false)
        {
            detectOut.enabled = false;
            detectInRoom.enabled = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Main_Controller player = collision.transform.parent.parent.GetComponent<Player_Main_Controller>();
        
        if (player != null && inBossRoom == false)
        {
            StartCoroutine(DoorClose());
        }
        if (player != null && inBossRoom == true)
        {
            StartCoroutine(DoorOpen());
        }

    }

    IEnumerator DoorClose()
    {
        yield return new WaitForSeconds(0.5f);
        isOpen = false;
        col.enabled = true;
        animator.SetBool("isOpen", isOpen);
        inBossRoom = true;
    }
    IEnumerator DoorOpen()
    {
        yield return null;
        isOpen = true;
        col.enabled = false;
        animator.SetBool("isOpen", isOpen);
        inBossRoom = false;
    }
}
