﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    //Resolution[] resolutions;
    //public Dropdown resolutioDropdown;
    public AudioMixer audioMixer;
    public Player_Main_Controller playerScript;
    void Start()
    {
        //resolutions = Screen.resolutions;

        //resolutioDropdown.ClearOptions();

        /*List<string> options = new List<string>();

        int currenResolutioIndex = 0;

        for(int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currenResolutioIndex = i;
            }
        }
        /*resolutioDropdown.AddOptions(options);
        resolutioDropdown.value = currenResolutioIndex;
        resolutioDropdown.RefreshShownValue();*/
    }

    /*public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
    }*/
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;

        Destroy(GameObject.Find("DontDestroyOnLoadData"));
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetEffectVolume(float volume)
    {
        AudioManager.SoundEffectsVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        MusicManager.MusicVolume = volume;
    }

    public void SetAutoAim(bool isActive)
    {
        playerScript.autoAimIsActive = isActive;
    }
}
