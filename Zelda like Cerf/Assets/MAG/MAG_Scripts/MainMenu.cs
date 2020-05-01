using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public Animator animator;

    public GameObject optionsFirstButton, optionClosedButton, continueButton, newGameMenu;

    private void Start()
    {
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
            }
            else
            {
                PlayGame();
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
        SceneManager.LoadScene(1);

        yield break;
    }

    public void QuitGame()
    {
        Debug.Log("oui oui ");
        Application.Quit();
    }

    public void LauchOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void BacktoPauseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionClosedButton);
    }
}
