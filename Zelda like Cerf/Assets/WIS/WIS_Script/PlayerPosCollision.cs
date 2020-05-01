using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerPosCollision : MonoBehaviour
{

    public enum PlayerPos { Village, Graveyard, Donjon01, Forest01, Forest02, Boss };

    public PlayerPos Position;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            switch (Position)
            {
                case PlayerPos.Village:
                    MusicManager.PlayerCurrentPos = MusicManager.PlayerPos.Village;
                    break;
                case PlayerPos.Graveyard:
                    MusicManager.PlayerCurrentPos = MusicManager.PlayerPos.Graveyard;
                    break;
                case PlayerPos.Donjon01:
                    MusicManager.PlayerCurrentPos = MusicManager.PlayerPos.Donjon01;
                    break;
                case PlayerPos.Forest01:
                    MusicManager.PlayerCurrentPos = MusicManager.PlayerPos.Forest01;
                    break;
                case PlayerPos.Forest02:
                    MusicManager.PlayerCurrentPos = MusicManager.PlayerPos.Forest02;
                    break;
                case PlayerPos.Boss:
                    MusicManager.PlayerCurrentPos = MusicManager.PlayerPos.Boss;
                    break;
                default:
                    break;
            }
        }
    }



}
