﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ronces : Elements_Controller
{
    public float timeBeforeDestruction;
    Rigidbody2D ronceRb;
    public bool isARonceEpaisse;
    public float shakeMagnitude;
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

    public void StartDestruction(float levelProjecting, float forceValueVersatilAttack)
    {
        StartCoroutine(Destruction(levelProjecting, forceValueVersatilAttack));
    }

    public IEnumerator Destruction(float levelProjecting, float forceValueVersatilAttack)
    {
        yield return new WaitForSeconds(0.5f);
        ronceRb.bodyType = RigidbodyType2D.Dynamic;
        StartTakeForce(forceValueVersatilAttack, levelProjecting);
        yield return new WaitForSeconds(timeBeforeDestruction);
        Destroy(gameObject);
    }
    
    public void StartCantDestroy(GameObject bullet)
    {
        StartCoroutine(CantDestroy(bullet));
    }
    public IEnumerator CantDestroy(GameObject bullet)
    {
        float angle = (transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
        Vector2 shakeDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        shakeDirection.Normalize();

        Vector2 initialTransform = transform.position;
        for (int i = 0; i <= 5; i++)
        {
            if(i%2 == 0) 
            {
                yield return new WaitForSeconds(0.15f);
                transform.position += (Vector3)shakeDirection*shakeMagnitude;
            }
            else
            {
                yield return new WaitForSeconds(0.15f);
                transform.position -= (Vector3)shakeDirection*shakeMagnitude;
            }
        }
       
        Destroy(bullet);
    }

}
