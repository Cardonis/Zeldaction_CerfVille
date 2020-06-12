using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTreeController : MonoBehaviour
{
    bool playerIsNear;
    Player_Main_Controller player;

    [HideInInspector] public List<RoomController> rooms;
    private float CurrentSoundEffectVolume;

    public OutlineController outlineController;

    public ParticleSystem healthParticleSystem;

    private AudioManager audiomanager;

    private void Start()
    {
        playerIsNear = false;

        GameObject[] rs = GameObject.FindGameObjectsWithTag("Room");

        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        foreach (GameObject r in rs)
        {
            rooms.Add(r.GetComponent<RoomController>());
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller p = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (p != null)
        {
            player = p;
            player.pressX.SetActive(true);
            playerIsNear = true;
            outlineController.outLining = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Player_Main_Controller player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            player.pressX.SetActive(false);
            playerIsNear = false;
            outlineController.outLining = false;
        }

    }
    private void Update()
    {
        if (Input.GetButtonDown("X") && playerIsNear == true)
        {

            audiomanager.Play("UI_Save");
            

            player.pressX.SetActive(false);
            outlineController.outLining = false;

            healthParticleSystem.Play();

            player.StartCoroutine(player.GainLife(player.maxLife - player.currentLife));

            for(int i = 0; i < rooms.Count; i++)
            {
                rooms[i].FullReset();
            }

            player.SavePlayer();
            
        }
    }
}
