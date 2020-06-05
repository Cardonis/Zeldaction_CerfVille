using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public MapCollumn[] fullClouds;
    public Vector2 roomsize;
    public Vector2 firstRoom;
    public Transform player;
    
    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        for (int i = 0; i < 7; i++)
        {
            for (int a = 0; a < 11; a++)
            {
                if (fullClouds[i].cloudCollumn[a] != null)
                {
                    fullClouds[i].cloudCollumn[a].SetActive(!data.mapDiscovery[i, a]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int GetPlayerRoomPosition()
    {
        Vector2 playerPosition = player.position;
        Vector2Int playerRoomPosition = Vector2Int.zero;

        while (playerPosition.x - roomsize.x >= firstRoom.x)
        {
            playerPosition.x -= roomsize.x;
            playerRoomPosition.x++;
        }
        while (playerPosition.y - roomsize.y >= firstRoom.y)
        {
            playerPosition.y -= roomsize.y;
            playerRoomPosition.y++;
        }
        return playerRoomPosition;
    }
    public void UpdateCloud()
    {
        Vector2Int playerRoomPosition = GetPlayerRoomPosition();
        if(fullClouds[playerRoomPosition.x].cloudCollumn[playerRoomPosition.y] != null)
        {
            fullClouds[playerRoomPosition.x].cloudCollumn[playerRoomPosition.y].SetActive(false);
        }
        
    }

    [System.Serializable]
    public class MapCollumn
    {
        public GameObject[] cloudCollumn;
    }
}
