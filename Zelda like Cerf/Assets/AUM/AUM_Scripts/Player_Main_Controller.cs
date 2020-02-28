using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main_Controller : MonoBehaviour
{
    public float speed;

    [Header("Base Attack")]

    public float durationBaseAttack;
    public float cooldownBaseAttack;
    float timerCooldownBaseAttack;
    public float speedKnockBackBaseAttack;

    Transform baseAttackCollidersParent;

    public Collider2D[] baseAttackColliders;
    //PlaceHolder pour le  moment, avec l'animation il y en aura pas besoin
    public SpriteRenderer[] baseAttackSRs;

    [Header("Versatil Attack")]

    public GameObject bulletVersatilAttack;
    public float speedBulletVersatil;
    public float cooldownVersatilAttack;
    float timerCooldownVersatilAttack;
    public float forceValueVersatilAttack;
    public float rangeMaxVersatilAttack;

    Transform vBullets;

    [HideInInspector] public bool projected = false;

    [HideInInspector] public Rigidbody2D rb;

    Vector2 input;
    [HideInInspector] public Vector2 direction;
    float directionAngle;




    void Start()
    {
        baseAttackCollidersParent = transform.Find("PColliders").Find("PAttackColliders").Find("PBaseAttackColliders");

        vBullets = GameObject.Find("VBullets").transform;

        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackSRs[i].enabled = false;
        }

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackColliders[i].enabled = false;
        }
    }


    void Update()
    {

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(!projected)
        {
            if (input.magnitude > 0)
            {
                direction = input;
                directionAngle = Vector2.Angle(transform.right, direction);

                if(direction.y < 0)
                {
                    directionAngle = -directionAngle;
                }

                rb.velocity = direction * speed * Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
        }
        
        if(timerCooldownBaseAttack < 0)
        {
            if (Input.GetButtonDown("A") && !projected)
            {
                StartCoroutine(BaseAttack());
            }
        }
        else
        {
            timerCooldownBaseAttack -= Time.fixedDeltaTime;
        }

        if (timerCooldownVersatilAttack < 0)
        {
            if (Input.GetButtonDown("X") && ! projected)
            {
                VersatilAttack();
            }
        }
        else
        {
            timerCooldownVersatilAttack -= Time.fixedDeltaTime;
        }

    }

    IEnumerator BaseAttack()
    {

        projected = true;

        baseAttackCollidersParent.rotation = Quaternion.Euler(0, 0, directionAngle);

        //Activation des Sprite Renderes
        for (int i = 0; i < baseAttackSRs.Length; i++)
        {
            baseAttackSRs[i].enabled = true;
        }

        //Activation de l'hitbox du sweetspot
        baseAttackColliders[0].enabled = true;

        yield return new WaitForSeconds(0.05f);

        //Activation de toutes les hitboxs
        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackColliders[i].enabled = true;
        }

        //Attack duration + knock back
        for(float i = durationBaseAttack; i > 0; i -= Time.fixedDeltaTime)
        {
            rb.velocity = -direction.normalized * speedKnockBackBaseAttack * Time.fixedDeltaTime;

            yield return null;
        }

        timerCooldownBaseAttack = cooldownBaseAttack;

        for (int i = 0; i < baseAttackSRs.Length; i++)
        {
            baseAttackSRs[i].enabled = false;
        }

        projected = false;

        baseAttackCollidersParent.rotation = Quaternion.identity;

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackColliders[i].enabled = false;
        }
    }

    void VersatilAttack()
    {
        Bullet_Versatil_Controller bullet = Instantiate(bulletVersatilAttack, vBullets).GetComponent<Bullet_Versatil_Controller>();

        bullet.transform.position = transform.position;

        projected = true;
        rb.velocity = new Vector2(0, 0);

        bullet.player = transform;
        bullet.maxDistance = rangeMaxVersatilAttack;

        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * speedBulletVersatil * Time.fixedDeltaTime;

        timerCooldownVersatilAttack = cooldownVersatilAttack;
    }

}
