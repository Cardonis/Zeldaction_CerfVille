using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int part;
    public int maxHealth;
    public int healthThirdNumber;
    public float[] position;
    public float[] confinerPosition;

    public bool[,] mapDiscovery = new bool[7, 11];

    public List<string> biomeNames = new List<string>();
    public List<string> roomNames = new List<string>();
    public List<bool> roomsCleared = new List<bool>();

    public List<bool> tiersDeCoeurCollected = new List<bool>();

    public List<bool> cinematiquePlayed = new List<bool>();

    public PlayerData (Player_Main_Controller player, MapDisplay map, List<RoomController> roomControllers)
    {

        part = player.part;
        maxHealth = player.maxLife;

        healthThirdNumber = player.inventoryBook.currentNumberOfHearthThird;



        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

        confinerPosition = new float[2];
        confinerPosition[0] = player.confiner.transform.position.x;
        confinerPosition[1] = player.confiner.transform.position.y;

        for (int i = 0; i < 7; i++)
        {
            for (int a = 0; a < 11; a++)
            {
                if( map.fullClouds[i].cloudCollumn[a] != null)
                {
                    mapDiscovery[i, a] = !map.fullClouds[i].cloudCollumn[a].activeSelf;
                }
            }
        }

        biomeNames.Clear();
        roomNames.Clear();
        roomsCleared.Clear();
        tiersDeCoeurCollected.Clear();
        cinematiquePlayed.Clear();

        for (int i = 0; i < roomControllers.Count; i++)
        {
            biomeNames.Add(roomControllers[i].biomeName);
            roomNames.Add(roomControllers[i].roomName);
            roomsCleared.Add(roomControllers[i].clear);

            if (roomControllers[i].tiersDecoeur != null)
                tiersDeCoeurCollected.Add(roomControllers[i].tiersDecoeur.collected);
            else
                tiersDeCoeurCollected.Add(false);

            if (roomControllers[i].timelineController != null)
                cinematiquePlayed.Add(roomControllers[i].timelineController.wasPlayed);
            else
                cinematiquePlayed.Add(false);
        }
    }
}
