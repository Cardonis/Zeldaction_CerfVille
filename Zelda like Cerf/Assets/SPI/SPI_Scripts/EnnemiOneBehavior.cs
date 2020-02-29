using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiOneBehavior : Ennemy_Controller
{
    [HideInInspector]

    public float ennemiSpeed;
    /*public float ennemiDamage;
    public int ennemiLife;
    public int ennemiLifeLeft;*/

    public float kockbackDistance;

    public float distanceToDash;
    public float timeDashCharge;
    public float recoveryTime;

    public float dashSpeed;

    private Vector3 targetPosition;

    public bool canMove=true;
    public bool canDash = true;

    public Color dashColor = Color.red;
    public Color normalColor = Color.white;

    override public void Start()
    {
        base.Start();

        player = GameObject.Find("Player").GetComponent<Player_Main_Controller>();
    }


    override public void Update()
    {
        base.Update();

        if (projected)
        {
            

            RegisterVelocity();
            return;
        }
        else
        {
            velocityValues.Clear();
        }
            

        if (Vector2.Distance(transform.position, player.transform.position) > distanceToDash && canMove == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, ennemiSpeed * Time.deltaTime);
        }

        else if (Vector2.Distance(transform.position, player.transform.position) <= distanceToDash && canMove == true)
        {
            canMove = false;
           
            StartCoroutine("EnnemiDash");
        }
    }

    IEnumerator EnnemiDash()
    {
        GetComponentInChildren<SpriteRenderer>().material.color = dashColor;
        yield return new WaitForSeconds(timeDashCharge);
        GetComponentInChildren<SpriteRenderer>().material.color = normalColor;
        targetPosition = new Vector2(player.transform.position.x, player.transform.position.y);

        while (transform.position != targetPosition && canDash == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.005f);
        }
        canDash = true;
        yield return new WaitForSeconds(recoveryTime);
        canMove = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       

        if (other.CompareTag("Player"))
        {
            canDash = true;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canDash = true;
        }
    }
}
