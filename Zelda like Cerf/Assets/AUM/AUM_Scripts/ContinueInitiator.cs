using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueInitiator : MonoBehaviour
{
    public Player_Main_Controller player;

    Continue continueManager;

    private void Start()
    {
        GameObject ddol = GameObject.Find("DontDestroyOnLoadData");

        if(ddol != null)
        {
            continueManager = ddol.GetComponent<Continue>();

            if (continueManager.continuying == true)
            {
                player.LoadPlayer();
            }
            else if(continueManager.continuying == false)
            {
                player.SavePlayer();
            }

            continueManager.continuying = false;
        }
        
        Destroy(gameObject);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
