using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBook : MonoBehaviour
{
    [HideInInspector] public int currentNumberOfHearthThird;

    public Player_Main_Controller player;

    public GameObject inventoryBookBackgroundUI, marquePageUI;
    public GameObject pauseMenuUi, mapUi, playerAndTipsUi;
    [HideInInspector] public bool iventoryIsOpen;
    public GameObject pauseFirstButton, playerAndTipsUIFirstButton, mapUIFirstButton;
    void Start()
    {
      currentNumberOfHearthThird = 0;
      iventoryIsOpen = false;
    }

    void Update()
    {
        if (currentNumberOfHearthThird == 3)
        {
            player.maxLife ++;
            player.currentLife += 2;
            currentNumberOfHearthThird = 0;
        }

        if (Input.GetButtonDown("Start") || Input.GetButtonDown("Y") || Input.GetButtonDown("B"))
        {
            if (iventoryIsOpen == false)
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
                    playerAndTipsUi.SetActive(true);
                    OpenPlayerAndTipsMenu();
                }
                if (Input.GetButtonDown("B"))
                {
                    mapUi.SetActive(true);
                    OpenMapMenu();
                }
            }
            
        }
        if(Input.GetButtonDown("Start") && iventoryIsOpen == true)
        {
            CloseInventory();
        }
    }

    public void OpenPauseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void OpenMapMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mapUIFirstButton);
    }

    public void OpenPlayerAndTipsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playerAndTipsUIFirstButton);
    }
    public void CloseInventory()
    {
        inventoryBookBackgroundUI.SetActive(false);
        iventoryIsOpen = false;
        marquePageUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CloseCurrent()
    {
        mapUi.SetActive(false);
        playerAndTipsUi.SetActive(false);
        pauseMenuUi.SetActive(false);
    }
}
