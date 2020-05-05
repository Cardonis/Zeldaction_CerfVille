using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caisse_Controller : Elements_Controller
{
    public bool isTractable;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        isTractable = true;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //StartCoroutine(audiomanager.PlayOne("Rock_Impact", gameObject));
    }

}
