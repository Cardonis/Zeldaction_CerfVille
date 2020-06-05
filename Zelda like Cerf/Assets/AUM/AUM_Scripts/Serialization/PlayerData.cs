using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int part;
    public int maxHealth;
    public float[] position;
    public float[] confinerPosition;
    public bool[,] mapDiscovery = new bool[7, 11];

    public PlayerData (Player_Main_Controller player, MapDisplay map)
    {

        part = player.part;
        maxHealth = player.maxLife;

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
    }
}
