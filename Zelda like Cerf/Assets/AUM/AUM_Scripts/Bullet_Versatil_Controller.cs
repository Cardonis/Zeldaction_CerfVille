using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Versatil_Controller : MonoBehaviour
{
    public LineRenderer lr;
    [HideInInspector] public Transform player;
    public Rigidbody2D rb;
    [HideInInspector] public float maxDistance;


    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, player.position);
        lr.SetPosition(1, transform.position);

        if (Vector2.Distance(transform.position, player.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Elements_Controller ec = collision.GetComponentInParent<Elements_Controller>();

        if (ec != null)
        {
            rb.velocity = new Vector2(0, 0);

            transform.SetParent(ec.transform);
            transform.position = collision.transform.position;
            StartCoroutine(ec.TakeForce(player, player.GetComponent<Player_Main_Controller>().forceValueVersatilAttack));
            
        }
    }
}
