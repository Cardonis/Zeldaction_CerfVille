using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<SpriteRenderer> limitSRs;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < limitSRs.Count; i++)
        {
            limitSRs[i].enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
