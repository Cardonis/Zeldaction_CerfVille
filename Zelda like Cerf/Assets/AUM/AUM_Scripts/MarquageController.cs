using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarquageController : MonoBehaviour
{
    [HideInInspector] public Player_Main_Controller player;
    public float marquageTimer;

    public SpriteRenderer sR;

    Color originalColor;
    float originalTimer;

    // Start is called before the first frame update
    void Start()
    {
        marquageTimer = player.marquageDuration;
        player.marquageManager.marquageControllers.Add(this);

        originalColor = sR.material.color;
        originalTimer = marquageTimer;

    }

    // Update is called once per frame
    void Update()
    {
        marquageTimer -= Time.deltaTime;

        sR.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, (marquageTimer + 0.5f) / (originalTimer + 0.5f));

        if(marquageTimer < 0)
        {
            player.marquageManager.marquageControllers.Remove(this);
            Destroy(gameObject);
        }
    }

}
