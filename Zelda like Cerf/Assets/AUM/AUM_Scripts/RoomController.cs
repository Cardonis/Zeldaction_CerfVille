using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<SpriteRenderer> limitSRs;
    public List<Collider2D> limitColliders;
    public List<Ennemy_Controller> ennemies;

    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < limitSRs.Count; i++)
        {
            limitSRs[i].enabled = false;
        }

        for (int i = 0; i < limitColliders.Count; i++)
        {
            limitColliders[i].enabled = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < ennemies.Count; i++)
        {
            if (ennemies[i] == null)
            {
                ennemies.RemoveAt(i);
            }
        }

        if (active == false)
        {
            for (int i = 0; i < ennemies.Count; i++)
            {
                ennemies[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < ennemies.Count; i++)
            {
                if(ennemies[i] != null)
                ennemies[i].enabled = true;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ennemy_Controller ec = collision.GetComponentInParent<Ennemy_Controller>();

        if (ec != null)
        {
            ennemies.Add(ec);
        }

        if (collision.attachedRigidbody.tag == "Player")
        {
            Player_Main_Controller pmc = collision.attachedRigidbody.GetComponent<Player_Main_Controller>();
            if (pmc != null)
            {
                pmc.confiner.StartCoroutine(pmc.confiner.Transition(this, pmc));
            }
        }
    }
}
