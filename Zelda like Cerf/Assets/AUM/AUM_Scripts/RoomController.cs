﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<SpriteRenderer> limitSRs;
    public List<Collider2D> limitColliders;
    [HideInInspector] public List<Ennemy_Controller> ennemies;

    public List<Door> doorsToClear;
    [HideInInspector] public List<Caisse_Controller> objectsToReset;

    public bool active = false;

    public bool clear = false;

    public bool monsterRoom = true;

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

        if(clear == false)
        {
            if (monsterRoom == true)
            {
                bool cleared = true;
                for (int i = 0; i < ennemies.Count; i++)
                {
                    if (ennemies[i].dead == false)
                    {
                        cleared = false;
                        break;
                    }
                }

                if (cleared == true)
                {
                    clear = true;
                }
            }
            else
            {
                bool cleared = true;
                for (int i = 0; i < doorsToClear.Count; i++)
                {
                    if (doorsToClear[i].isOpen == false)
                    {
                        cleared = false;
                        break;
                    }
                }

                if (cleared == true)
                {
                    clear = true;
                }
            }
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ennemy_Controller ec = collision.GetComponentInParent<Ennemy_Controller>();

        Caisse_Controller elc = collision.GetComponentInParent<Caisse_Controller>();

        if (ec != null)
        {
            bool canAdd = true;

            for(int i = 0; i < ennemies.Count; i++)
            {
                if(ec == ennemies[i])
                {
                    canAdd = false;
                    break;
                }

            }

            if(canAdd == true)
                ennemies.Add(ec);
        }

        if (elc != null)
        {
            bool canAdd = true;

            for (int i = 0; i < objectsToReset.Count; i++)
            {
                if (elc == objectsToReset[i])
                {
                    canAdd = false;
                    break;
                }

            }

            if (canAdd == true)
                objectsToReset.Add(elc);
        }

        if (collision.attachedRigidbody.tag == "Player")
        {
            Player_Main_Controller pmc = collision.attachedRigidbody.GetComponent<Player_Main_Controller>();
            if (pmc != null)
            {
                pmc.confiner.StartCoroutine(pmc.confiner.Transition(this, pmc));

                if(monsterRoom == true)
                {
                    if(clear == false)
                    {
                        for(int i = 0; i < ennemies.Count; i++)
                        {                           
                            ennemies[i].gameObject.SetActive(true);

                            ennemies[i].dead = false;

                            ennemies[i].playerDetected = false;

                            ennemies[i].pv = ennemies[i].initialLife;
                            ennemies[i].transform.position = ennemies[i].initialPosition;
                            ennemies[i].rb.velocity = Vector2.zero;
                        }

                        for(int i = 0; i < objectsToReset.Count; i++)
                        {
                            objectsToReset[i].transform.position = objectsToReset[i].initialPosition;
                            if (objectsToReset[i].spawned == true)
                            {
                                objectsToReset.Remove(objectsToReset[i]);
                                Destroy(objectsToReset[i].gameObject);
                            }
                                
                        }
                    }
                }
                else
                {
                    if (clear == false)
                    {
                        

                        for (int i = 0; i < objectsToReset.Count; i++)
                        {
                            objectsToReset[i].transform.position = objectsToReset[i].initialPosition;
                            if (objectsToReset[i].spawned == true)
                            {
                                objectsToReset.Remove(objectsToReset[i]);
                                Destroy(objectsToReset[i].gameObject);
                            }

                        }
                    }

                    for (int i = 0; i < ennemies.Count; i++)
                    {
                        if (ennemies[i].dead == false)
                            ennemies[i].transform.position = ennemies[i].initialPosition;

                        ennemies[i].playerDetected = false;

                        ennemies[i].pv = ennemies[i].initialLife;
                    }
                }
            }
        }
    }


}
