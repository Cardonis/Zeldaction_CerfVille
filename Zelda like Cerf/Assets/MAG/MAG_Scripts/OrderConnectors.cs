using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderConnectors : MonoBehaviour
{
    public Buttonmanager[] allButtons;
    int numberOfPressedButtons;
    [HideInInspector] public bool openDoor;
    void Start()
    {
        
        openDoor = false;
    }


    void Update()
    {
        numberOfPressedButtons = 0;
        for (int i = 0; i < allButtons.Length; i++)
        {
            if (allButtons[i].isPressed == true && i != 0)
            {
                if (allButtons[i - 1].isPressed == true)
                {
                    allButtons[i].isPressed = true;
                    numberOfPressedButtons++;
                }
                else
                {
                    for (int b = 0; b < allButtons.Length; b++)
                    {
                        allButtons[b].isPressed = false;
                    }
                }
            }

        }
        if (numberOfPressedButtons == allButtons.Length-1)
        {
            openDoor = true;
        }

    }

}
