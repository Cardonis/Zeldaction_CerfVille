using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoor : MonoBehaviour
{
    public List<Ennemy_Controller> ennemyToOpen;

    SpriteRenderer sr;
    Collider2D col;
    Animator animator;
    bool readyToOpen = false;
    public bool isTrigger;

    public List<GameObject> particleSystems;

    public GameObject particleSystemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        bool readyToOpen = true;

        for (int i = 0; i < ennemyToOpen.Count; i++)
        {
            if (ennemyToOpen[i].gameObject.activeSelf == true)
            {
                readyToOpen = false;

                break;
            }
        }

        if (readyToOpen == true)
        {
            col.enabled = false;

        }

        if (isTrigger == true)
        {
            animator.SetBool("isOpen", readyToOpen);

            for (int i = 0; i < particleSystems.Count; i++)
            {
                particleSystems[i].SetActive(!readyToOpen);
            }
        }
        else
        {
            animator.SetBool("isOpen", true);
        }
            
    }

}
