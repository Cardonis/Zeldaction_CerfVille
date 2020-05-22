using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBlocks : MonoBehaviour
{
    public Collider2D col;
    public SpriteRenderer sR;

    public Player_Main_Controller player;

    public int partToBlock;

    Animator animator;
    public bool hasFall;
    public bool isFalling;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hasFall = false;
        isFalling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.part == partToBlock)
        {
            
            col.enabled = true;
            isFalling = true;
            hasFall = true;
        }
        else
        {
            col.enabled = false;

        }
        animator.SetBool("hasFall", hasFall);
        animator.SetBool("isFalling", isFalling);
    }
}
