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
    void Start()
    {
        timerIsActive = false;
        openDoor = false;
        numberOfActiveButtons = 0;
        timer = initialTimer;
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
