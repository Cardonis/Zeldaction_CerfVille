using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public int partLevel;
    public GameObject arbusteAmiMortPetit;
    public GameObject arbusteAmiMortGrand;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Main_Controller pmc = collision.attachedRigidbody.GetComponent<Player_Main_Controller>();

        if(pmc != null)
        {
            if(pmc.part < partLevel)
            pmc.part = partLevel;
            arbusteAmiMortPetit.SetActive(false);
            arbusteAmiMortGrand.SetActive(true);
        }
    }
}
