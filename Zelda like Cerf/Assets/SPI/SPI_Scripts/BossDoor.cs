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

    void Start()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        isInTheRoom = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Main_Controller player = collision.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null && isInTheRoom == false)
        {
            StartCoroutine(DoorClose());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player_Main_Controller player = collision.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null && isInTheRoom)
        {
            isOpen = true;
            col.enabled = false;
            animator.SetBool("isOpen", isOpen);
            isInTheRoom = false;
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
