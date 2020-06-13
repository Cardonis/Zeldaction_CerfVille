using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbreAyen : MonoBehaviour
{

    public GameObject arbusteAmiMortPetit;
    public GameObject arbusteAmiMortGrand;
    public Player_Main_Controller player;

    private void Start()
    {
        if (player.part >= 2)
        {
            arbusteAmiMortPetit.SetActive(false);
            arbusteAmiMortGrand.SetActive(true);

        }
    }
}
