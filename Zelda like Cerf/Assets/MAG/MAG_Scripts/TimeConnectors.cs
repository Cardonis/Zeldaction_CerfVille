using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeConnectors : MonoBehaviour
{
    public Buttonmanager[] allButtons;
    float timer;
    public float initialTimer;
    bool timerIsActive;
    int counter;
    int numberOfActiveButtons;
    [HideInInspector] public bool openDoor;

    AudioManager audiomanager;

    private bool TemporalIsPlaying;

    void Start()
    {
        timerIsActive = false;
        openDoor = false;
        numberOfActiveButtons = 0;
        timer = initialTimer;
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        TemporalIsPlaying = false;
    }
    void Update()
    {
        for (counter = 0; counter < allButtons.Length; counter++)
        {

            if (allButtons[counter].isPressed == true)
            {
                if (allButtons[counter].lastStarting != null)
                    allButtons[counter].StopCoroutine(allButtons[counter].lastStarting);

                allButtons[counter].lastStarting = allButtons[counter].StartCoroutine(allButtons[counter].StartLineLit(1f, 0f, initialTimer - 0.5f));
                timerIsActive = true;
                counter = allButtons.Length + 1;
            }
        }

        

        if (timerIsActive == true)
        {
            if (TemporalIsPlaying == false)
            {
                StartCoroutine(audiomanager.PlayAndFadeOut("Temporal01", gameObject, initialTimer));
                TemporalIsPlaying = true;
            }
            timer -= Time.deltaTime;
            numberOfActiveButtons = 0;

            foreach (Buttonmanager buttons in allButtons)
            {
                if (buttons.isPressed == true)
                {
                    numberOfActiveButtons += 1;
                }

            }

            if (timer < 0 && numberOfActiveButtons != allButtons.Length)
            {
                foreach (Buttonmanager buttonsActivated in allButtons)
                {
                    if (buttonsActivated.lastStarting != null)
                    {
                        buttonsActivated.StopCoroutine(buttonsActivated.lastStarting);
                        buttonsActivated.lastStarting = null;
                    }

                    if (buttonsActivated.lastLineLit != null)
                    {
                        buttonsActivated.StopCoroutine(buttonsActivated.lastLineLit);
                        buttonsActivated.lastLineLit = null;
                    }

                    buttonsActivated.dissolveMaterial.material.SetFloat("_Fade", 0f);

                    buttonsActivated.isPressed = false;
                    StartCoroutine(audiomanager.PlayOne("ButtonOff", gameObject));
                }
                timer = initialTimer;
                timerIsActive = false;
                TemporalIsPlaying = false;
            }

            if (numberOfActiveButtons == allButtons.Length)
            {
                foreach (Buttonmanager buttonsActivated in allButtons)
                {
                    if (buttonsActivated.lastStarting != null)
                    {
                        buttonsActivated.StopCoroutine(buttonsActivated.lastStarting);
                        buttonsActivated.lastStarting = null;
                    }

                    if (buttonsActivated.lastLineLit != null)
                    {
                        buttonsActivated.StopCoroutine(buttonsActivated.lastLineLit);
                        buttonsActivated.lastLineLit = null;
                    }

                    buttonsActivated.lastStarting = buttonsActivated.StartCoroutine(buttonsActivated.StartLineLit(buttonsActivated.dissolveMaterial.material.GetFloat("_Fade"), 1f, 0.5f));
                }

                timerIsActive = false;
                openDoor = true;
            }
        }
    }
}
