using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caisse_Controller : Elements_Controller
{
    public bool isTractable;
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
            StartCoroutine(audiomanager.PlayOneOne(isPlaying));
        }

    }

    public IEnumerator PlayOneOne(bool isPlaying)
    {
        isPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        isPlaying = false;

    }

}
