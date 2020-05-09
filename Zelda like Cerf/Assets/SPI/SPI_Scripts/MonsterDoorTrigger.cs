using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoorTrigger : MonoBehaviour
{
    public MonsterDoor monsterDoor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player"))
            monsterDoor.isTrigger = true;
    }
}
