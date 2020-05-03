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
    void Start()
    {
        timerIsActive = false;
        openDoor = false;
        numberOfActiveButtons = 0;
        timer = initialTimer;
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    void Update()
    {
        for (counter = 0; counter < allButtons.Length; counter++)
        {

            if (allButtons[counter].isPressed == true)
            {
                timerIsActive = true;
                counter = allButtons.Length + 1;
            }
        }

        

        if (timerIsActive == true)
        {

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
                    buttonsActivated.isPressed = false;
                    StartCoroutine(audiomanager.PlayOne("ButtonOff", gameObject));
                }
                timer = initialTimer;
                timerIsActive = false;
            }

            if (numberOfActiveButtons == allButtons.Length)
            {
                timerIsActive = false;
                openDoor = true;
            }
        }
    }
}
