using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public int part;
    public int maxHealth;
    public int currentLife;
    public float[] position;
    public float[] confinerPosition;
    public int currentScene;

    public PlayerData (Player_Main_Controller player)
    {
        part = player.part;
        maxHealth = player.maxLife;
        currentLife = player.currentLife;
        currentScene = SceneManager.GetActiveScene().buildIndex;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

        confinerPosition = new float[2];
        confinerPosition[0] = player.confiner.transform.position.x;
        confinerPosition[1] = player.confiner.transform.position.y;
    }
}
