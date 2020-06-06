using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeleporterInitiator : MonoBehaviour
{
    public List<Transform> teleportTransforms;

    TeleporterManager teleporterManager;

    public Player_Main_Controller player;

    void Awake()
    {
        GameObject ddol = GameObject.Find("DontDestroyOnLoadData");

        if (ddol != null)
        {
            teleporterManager = ddol.GetComponent<TeleporterManager>();
            if (teleporterManager.canTeleport == true)
            {
                player.LoadPlayer(teleporterManager.saveToTeleport);

                teleporterManager.canTeleport = false;
            }
        }

        Destroy(gameObject);
    }
}
