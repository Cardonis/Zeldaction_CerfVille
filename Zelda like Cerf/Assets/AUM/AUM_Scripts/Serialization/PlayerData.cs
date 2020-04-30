using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int part;
    public int maxHealth;
    public float[] position;

    public PlayerData (Player_Main_Controller player)
    {
        part = player.part;
        maxHealth = player.maxLife;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
    }
}
