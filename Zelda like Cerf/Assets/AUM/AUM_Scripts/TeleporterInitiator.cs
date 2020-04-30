using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeleporterInitiator : MonoBehaviour
{
    public List<Transform> teleportTransforms;

    TeleporterManager teleporterManager;

    public Player_Main_Controller player;

    private void Start()
    {
        teleporterManager = GameObject.Find("TeleporterManager").GetComponent<TeleporterManager>();

        if(teleporterManager.canTeleport == true)
        {
            player.transform.position = teleportTransforms[teleporterManager.teleporterNumber].transform.position;
            player.confiner.transform.position = teleportTransforms[teleporterManager.teleporterNumber].position;
            player.part = teleporterManager.teleportPart;
        }

        Destroy(gameObject);

    }
}
