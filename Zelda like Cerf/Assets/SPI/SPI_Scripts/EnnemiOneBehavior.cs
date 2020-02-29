using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiOneBehavior : MonoBehaviour
{
    [HideInInspector]
    public Transform player;

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

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > distanceToDash && canMove == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, ennemiSpeed * Time.deltaTime);
        }

        else if (Vector2.Distance(transform.position, player.position) <= distanceToDash && canMove == true)
        {
            canMove = false;
           
            StartCoroutine("EnnemiDash");
        }
    }

    IEnumerator EnnemiDash()
    {
        GetComponent<SpriteRenderer>().material.color = dashColor;
        yield return new WaitForSeconds(timeDashCharge);
        GetComponent<SpriteRenderer>().material.color = normalColor;
        targetPosition = new Vector2(player.position.x, player.position.y);

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
