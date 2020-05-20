using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarquageController : MonoBehaviour
{
    [HideInInspector] public Player_Main_Controller player;
    public float marquageTimer;

    public LineRenderer lr;

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

        lr.SetPosition(0, player.transform.position);
        lr.SetPosition(1, transform.position);

        if (marquageTimer < 0)
        {
            elementsAttachedTo.outlineController.outLining = false;

            player.pressX.SetActive(false);

            player.marquageManager.marquageControllers.Remove(this);
            Destroy(gameObject);
        }
    }

}
