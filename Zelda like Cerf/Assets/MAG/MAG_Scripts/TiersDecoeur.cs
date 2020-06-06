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

    [HideInInspector] public bool collected = false;

    AudioManager audioManager;
    private void Start()
    {
        playerIsNear = false;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.attachedRigidbody != null)
        player = collider.attachedRigidbody.GetComponent<Player_Main_Controller>();
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
        if (collider.attachedRigidbody != null)
            player = collider.attachedRigidbody.GetComponent<Player_Main_Controller>();
        if (player != null)
        {
            pressX.SetActive(false);
            playerIsNear = false;

            outlineController.outLining = false;
        }
    }

    void Update()
    {
        if (collected == true && desactivating == false) 
        {
            gameObject.SetActive(false);
        }

        if(playerIsNear == true && Input.GetButtonDown("X") && collected == false)
        {
            collected = true;

            if (inventoryBook.currentNumberOfHearthThird != 2)
                inventoryBook.StartCoroutine(HeartThirdShow());

            inventoryBook.currentNumberOfHearthThird++;

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
        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = Color.grey;
        Color c = player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color;

        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].fillAmount = (1f / 3f) * inventoryBook.currentNumberOfHearthThird;

        while (elapsedTime < 4f )
        {
            player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].fillAmount = (1f / 3f) * inventoryBook.currentNumberOfHearthThird;

            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / (4f));
            player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = c;
            yield return null;
        }

        c.a = 1.0f;
        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = c;

        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].color = Color.white;

        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].fillAmount = 1f;

        player.lifeDisplay.hearths[player.lifeDisplay.numberOfHearts].gameObject.SetActive(false);
    }

    IEnumerator DesactivationAfter(float time)
    {
        desactivating = true;
        audioManager.Play("Item_Coeur");
        sR.gameObject.SetActive(false);

        lightt.SetActive(false);

        yield return new WaitForSeconds(time);

        sR.gameObject.SetActive(true);

        lightt.SetActive(true);

        desactivating = false;

        gameObject.SetActive(false);
    }
}
