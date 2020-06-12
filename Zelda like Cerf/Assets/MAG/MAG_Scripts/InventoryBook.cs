using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBook : MonoBehaviour
{
    [HideInInspector] public int currentNumberOfHearthThird = 0;

    public Player_Main_Controller player;

    public GameObject inventoryBookBackgroundUI, marquePageUI;
    public GameObject pauseMenuUi, mapUi, playerAndTipsUi;
    [HideInInspector] public bool iventoryIsOpen;
    public GameObject pauseFirstButton, playerAndTipsUIFirstButton, mapUIFirstButton;
    public GameObject emptyHeart, notSoEmptyHeart, notNotSoEmptyHeart;

    public bool tipsMenu; 
    void Start()
    {
      iventoryIsOpen = false;
    }

    void Update()
    {
        if (currentNumberOfHearthThird == 3)
        {
            player.maxLife += 2;
            player.currentLife += 2;
            currentNumberOfHearthThird = 0;
            notNotSoEmptyHeart.SetActive(false);
            emptyHeart.SetActive(true);
        }
        if (currentNumberOfHearthThird == 1)
        {
            emptyHeart.SetActive(false);
            notSoEmptyHeart.SetActive(true);
        }
        if (currentNumberOfHearthThird == 2)
        {
            notNotSoEmptyHeart.SetActive(true);
            notSoEmptyHeart.SetActive(false);
        }

        if ((Input.GetButtonDown("Start") || Input.GetButtonDown("Y")) && iventoryIsOpen == true)
        {
            StartCoroutine(CloseInventoryIn());
        }

        if ((Input.GetButtonDown("Start") || Input.GetButtonDown("Y")) && iventoryIsOpen == false)
        {
            Time.timeScale = 0f;
            inventoryBookBackgroundUI.SetActive(true);
            marquePageUI.SetActive(true);
            iventoryIsOpen = true;


            if (Input.GetButtonDown("Start"))
            {
                pauseMenuUi.SetActive(true);
                OpenPauseMenu();

            }
            if (Input.GetButtonDown("Y"))
            {
                mapUi.SetActive(true);
                OpenMapMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        tipsMenu = false;
    }

    public void OpenMapMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mapUIFirstButton);
        tipsMenu = false;
    }

    public void OpenPlayerAndTipsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playerAndTipsUIFirstButton);
        tipsMenu = true;
    }
    public void CloseInventory()
    {
        inventoryBookBackgroundUI.SetActive(false);
        iventoryIsOpen = false;
        marquePageUI.SetActive(false);
        Time.timeScale = 1f;
    }
    IEnumerator CloseInventoryIn()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        CloseCurrent();
        CloseInventory();
    }
    public void CloseCurrent()
    {
        mapUi.SetActive(false);
        playerAndTipsUi.SetActive(false);
        pauseMenuUi.SetActive(false);
    }
    public void CloseInventoryAll()
    {
        StartCoroutine(CloseInventoryIn());
    }
}
