﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiersDecoeur : MonoBehaviour
{

    public GameObject pressX;
    private Player_Main_Controller player;
    public InventoryBook inventoryBook;
    bool playerIsNear;

    private void Start()
    {
        playerIsNear = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            pressX.SetActive(true);
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            pressX.SetActive(false);
            playerIsNear = false;
        }
    }

    void Update()
    {
        if(playerIsNear == true && Input.GetButtonDown("X"))
        {
            inventoryBook.currentNumberOfHearthThird++;
            pressX.SetActive(false);
            Destroy(gameObject);
        }
    }
}
