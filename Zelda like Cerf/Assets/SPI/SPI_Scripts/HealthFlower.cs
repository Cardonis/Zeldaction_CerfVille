using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlower : MonoBehaviour
{
    public OutlineController outlineController;

    public ParticleSystem healthParticleSystem;

    public SpriteRenderer sR;

    public GameObject lightt;

    bool playerIsNear;
    Player_Main_Controller player;

    public int healValue;

    bool desactivating = false;

    private void Start()
    {
        playerIsNear = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.attachedRigidbody != null)
        {
            Player_Main_Controller p = collider.attachedRigidbody.GetComponent<Player_Main_Controller>();
            if (p != null)
            {
                player = p;
                if (player.currentLife != player.maxLife)
                    player.pressX.SetActive(true);
                playerIsNear = true;
                outlineController.outLining = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.attachedRigidbody != null)
        {
            Player_Main_Controller player = collider.attachedRigidbody.GetComponent<Player_Main_Controller>();
            if (player != null)
            {
                player.pressX.SetActive(false);
                playerIsNear = false;
                outlineController.outLining = false;
            }
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("X") && playerIsNear == true)
        {
            if (player.currentLife != player.maxLife && desactivating == false)
            {
                player.pressX.SetActive(false);
                player.StartCoroutine(player.GainLife(healValue));

                StartCoroutine(DesactivationAfter(2f));

                healthParticleSystem.Play();

                outlineController.outLining = false;
            }
        }
    }

    IEnumerator DesactivationAfter(float time)
    {
        desactivating = true;

        sR.gameObject.SetActive(false);

        lightt.SetActive(false);

        yield return new WaitForSeconds(time);

        sR.gameObject.SetActive(true);

        lightt.SetActive(true);

        desactivating = false;

        gameObject.SetActive(false);
    }
}
