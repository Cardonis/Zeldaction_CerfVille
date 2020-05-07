using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBlocks : MonoBehaviour
{
    public Collider2D col;
    public SpriteRenderer sR;

    public Player_Main_Controller player;

    public int partToBlock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.part == partToBlock)
        {
            col.enabled = true;
            sR.enabled = true;
        }
        else
        {
            col.enabled = false;
            sR.enabled = false;
        }
    }
}
