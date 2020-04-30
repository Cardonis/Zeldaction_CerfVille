using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTreeController : MonoBehaviour
{
    bool playerIsNear;
    public GameObject pressX;
    Player_Main_Controller player;

    [HideInInspector] public List<RoomController> rooms;

    private void Start()
    {
        playerIsNear = false;

        GameObject[] rs = GameObject.FindGameObjectsWithTag("Room");

        foreach(GameObject r in rs)
        {
            rooms.Add(r.GetComponent<RoomController>());
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        pressX.SetActive(true);
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
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
    private void Update()
    {
        if (Input.GetButtonDown("X") && playerIsNear == true)
        {

            pressX.SetActive(false);
            player.currentLife = player.maxLife;

            for(int i = 0; i < rooms.Count; i++)
            {
                rooms[i].FullReset();
            }

            //Save
        }
    }
}
