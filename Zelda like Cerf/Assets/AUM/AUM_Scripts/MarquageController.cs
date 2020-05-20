using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarquageController : MonoBehaviour
{
    [HideInInspector] public Player_Main_Controller player;
    public float marquageTimer;

    public SpriteRenderer sR;

    float originalTimer;

    [HideInInspector] public bool venom = false;

    public Elements_Controller elementsAttachedTo;

    // Start is called before the first frame update
    void Start()
    {
        marquageTimer = player.marquageDuration;
        player.marquageManager.marquageControllers.Add(this);

        originalTimer = marquageTimer;

    }

    // Update is called once per frame
    void Update()
    {
        marquageTimer -= Time.deltaTime;

        elementsAttachedTo.outlineController.outLining = true;

        if(marquageTimer < 0)
        {
            elementsAttachedTo.outlineController.outLining = false;

            player.marquageManager.marquageControllers.Remove(this);
            Destroy(gameObject);
        }
    }

}
