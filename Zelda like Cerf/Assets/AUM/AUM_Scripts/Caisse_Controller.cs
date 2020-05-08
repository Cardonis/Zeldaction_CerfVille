using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caisse_Controller : Elements_Controller
{
    private bool isPlaying;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        isTractable = true;
        isPlaying = false;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (isPlaying == false)
        {
            StartCoroutine(audiomanager.PlayOne("Rock_Impact", gameObject));
            StartCoroutine(PlayOneOne());
        }

    }

    public IEnumerator PlayOneOne()
    {
        isPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        isPlaying = false;
    }

}
