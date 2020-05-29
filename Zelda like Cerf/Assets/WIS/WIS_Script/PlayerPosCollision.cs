using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerPosCollision : MonoBehaviour
{

    //Le Script qui gère la position du joueur pour savoir quelle musiques et ambiances doivent être jouées.
    //Quand le joueur entre en collision avec cet objet, cela update sa position. 
    //Pour créer les différent position manager, il suffit de changer la position de ce script dans l'inspecteur.

    public enum PlayerPos { Village, Graveyard, Donjon01, Forest01, Forest02, Boss };

    public PlayerPos Position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.gameObject.tag == "Player" && MusicManager.Encounter == false)
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
