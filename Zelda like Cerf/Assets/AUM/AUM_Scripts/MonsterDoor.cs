using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoor : MonoBehaviour
{
    public List<Ennemy_Controller> ennemyToOpen;

    SpriteRenderer sr;
    Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < ennemyToOpen.Count; i++)
        {
            if(ennemyToOpen[i] == null)
            {
                ennemyToOpen.RemoveAt(i);
            }
        }

        if(ennemyToOpen.Count == 0)
        {
            sr.enabled = false;
            col.enabled = false;
        }
    }
}
