using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiersDecoeur : MonoBehaviour
{
    public OutlineController outlineController;

    public ParticleSystem healthParticleSystem;

    public SpriteRenderer sR;

    public GameObject lightt;

    GameObject pressX;
    private Player_Main_Controller player;
    InventoryBook inventoryBook;
    bool playerIsNear;

    bool desactivating = false;

    bool collected = false;

    private void Start()
    {
        playerIsNear = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            inventoryBook = player.inventoryBook;
            pressX = player.pressX;
            pressX.SetActive(true);
            playerIsNear = true;
            outlineController.outLining = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        player = collider.transform.parent.parent.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            pressX.SetActive(false);
            playerIsNear = false;

            outlineController.outLining = false;
        }
    }

    void Update()
    {
        if(playerIsNear == true && Input.GetButtonDown("X") && collected == false)
        {
            collected = true;

            inventoryBook.currentNumberOfHearthThird++;

            inventoryBook.StartCoroutine(HeartThirdShow());

            healthParticleSystem.Play();

            StartCoroutine(DesactivationAfter(2f));

            pressX.SetActive(false);

            outlineController.outLining = false;
        }
    }

    IEnumerator HeartThirdShow()
    {
        float elapsedTime = 0.0f;

        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].gameObject.SetActive(true);
        Color c = player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color;

        while (elapsedTime < 4f * inventoryBook.currentNumberOfHearthThird)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / (4f * inventoryBook.currentNumberOfHearthThird));
            player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = c;
            yield return null;
        }

        c.a = 1.0f;
        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = c;

        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].gameObject.SetActive(false);
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
