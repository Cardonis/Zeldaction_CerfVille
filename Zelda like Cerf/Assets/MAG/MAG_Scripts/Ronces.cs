using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ronces : Elements_Controller
{
    public float timeBeforeDestruction;
    Rigidbody2D ronceRb;
    public override void Start()
    {
        base.Start();
        ronceRb = GetComponent<Rigidbody2D>();
        ronceRb.bodyType = RigidbodyType2D.Static;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public void StartDestruction()
    {
        StartCoroutine(Destruction());
    }

    public IEnumerator Destruction()
    {
        yield return new WaitForSeconds(0.5f);
        ronceRb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(timeBeforeDestruction);
        Destroy(gameObject);
    }

}
