using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoorTrigger : MonoBehaviour
{
    public MonsterDoor monsterDoor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player") && monsterDoor.isTrigger == false)
        {
            monsterDoor.isTrigger = true;
            for(int i = 0; i < monsterDoor.ennemyToOpen.Count; i++)
            {
                monsterDoor.particleSystems.Add(Instantiate(monsterDoor.particleSystemPrefab, monsterDoor.ennemyToOpen[i].transform));
            }
            
        }

    }
}
