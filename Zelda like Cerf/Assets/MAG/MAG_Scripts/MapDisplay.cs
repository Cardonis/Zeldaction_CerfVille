using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public MapCollumn[] fullClouds;
    public Vector2 roomsize;
    public Vector2 firstRoom;
    public Transform player;
    public GameObject playerFace;
    private RectTransform playerFaceTransform;
    private Vector2 initialFacePos;
    void Start()
    {
        playerFaceTransform = playerFace.GetComponent<RectTransform>();
        initialFacePos = playerFaceTransform.anchoredPosition;
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
        //playerFaceTransform.anchoredPosition = fullClouds[playerRoomPosition.x].cloudCollumn[playerRoomPosition.y].GetComponent<RectTransform>().anchoredPosition;
        playerFaceTransform.anchoredPosition = new Vector2(initialFacePos.x + 68 * playerRoomPosition.x, initialFacePos.y + 46f * playerRoomPosition.y);
    }

    [System.Serializable]
    public class MapCollumn
    {
        public GameObject[] cloudCollumn;
    }
}
