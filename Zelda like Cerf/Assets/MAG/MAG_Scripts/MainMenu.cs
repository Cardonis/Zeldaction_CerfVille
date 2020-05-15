﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public Animator animator;

    public GameObject partsFirstButton, mainMenuFirstButton, optionsFirstButton, newGameFirstButton, continueButton, newGameMenu;

    private void Start()
    {
        BacktoMainMenu();

        if(continueButton != null)
        {
            string path = Application.persistentDataPath + "/player.save";
            if (File.Exists(path))
            {
                continueButton.SetActive(true);
            }
            else
            {
                continueButton.SetActive(false);
            }
        }
        
    }

    public void NewGame()
    {
        if (continueButton != null)
        {
            string path = Application.persistentDataPath + "/player.save";
            if (File.Exists(path))
            {
                newGameMenu.SetActive(true);
                gameObject.SetActive(false);
                LaunchNewGame();
            }
            else
            {
                StartNewGame();
            }
        }
        else
        {
            PlayGame();
        }
    }

    public void PlayGame()
    {
        StartCoroutine(PlayingGame());
    }

    IEnumerator PlayingGame()
    {
        animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        Time.timeScale = 1f;
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
        {
            Debug.Log("éf");
            SceneManager.LoadScene(data.currentScene);
        }
        else
        {
            SceneManager.LoadScene(1);
        }

        yield break;
    }

    public void QuitGame()
    {
        Debug.Log("oui oui ");
        Application.Quit();
    }

    public void LaunchOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void LaunchNewGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameFirstButton);
    }

    public void LaunchParts()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(partsFirstButton);
    }

    public void BacktoMainMenu()
    {
        if(mainMenuFirstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        }
        
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(2);
    }
}
