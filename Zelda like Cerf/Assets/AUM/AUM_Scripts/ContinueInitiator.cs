using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueInitiator : MonoBehaviour
{
    public Player_Main_Controller player;

    Continue continueManager;

    private void Start()
    {
        continueManager = GameObject.Find("DontDestroyOnLoadData").GetComponent<Continue>();

        if (continueManager.continuying == true)
        {
            player.LoadPlayer();
        }

        Destroy(gameObject);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
