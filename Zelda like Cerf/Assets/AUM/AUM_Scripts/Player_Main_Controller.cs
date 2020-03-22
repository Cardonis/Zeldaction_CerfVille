using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main_Controller : MonoBehaviour
{
    [Range(100.0f, 400.0f)] public float speed;

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
    public float chargeTime;
    float chargeTimer;
    bool multipleAttack = false;
    float chargeSpeedMultiplicator;
    public float forceValueVersatilAttack;
    public float rangeMaxVersatilAttack;
    public float marquageDuration;

    [HideInInspector] public bool canSpringAttack;

    public List<float> levelMultiplicator = new List<float>();

    //PlaceHolder pour le  moment, avec l'animation il y en aura pas besoin
    GameObject barsCharge;
    Transform barCharge;

    Transform vBullets;

    Transform arrowDirection;

    [HideInInspector] public bool projected = false;

    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public MarquageManager marquageManager;

    Vector2 input;
    [HideInInspector] public Vector2 direction;
    float directionAngle;

    void Start()
    {
        baseAttackCollidersParent = transform.Find("PColliders").Find("PAttackColliders").Find("PBaseAttackColliders");

        arrowDirection = transform.Find("Arrow");

        barsCharge = transform.Find("BarsCharge").gameObject;

        barCharge = barsCharge.transform.Find("BarCharge");

        vBullets = GameObject.Find("VBullets").transform;

        marquageManager = GameObject.Find("MarquageManager").GetComponent<MarquageManager>();

        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackSRs[i].enabled = false;
        }

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackColliders[i].enabled = false;
        }

        barsCharge.SetActive(false);
    }


    void Update()
    {

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (!projected)
        {
            if (input.magnitude > 0)
            {
                direction = input;


                rb.velocity = direction * speed * chargeSpeedMultiplicator * Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
        
        }

        if (timerCooldownBaseAttack < 0)
        {
            Debug.Log(canSpringAttack);
            if ( Input.GetButtonDown("A") && (!projected || canSpringAttack) )
            {
                StartCoroutine(BaseAttack());
            }
        }
        else
        {
            timerCooldownBaseAttack -= Time.deltaTime;
        }

        if (timerCooldownVersatilAttack < 0)
        {
            if(!projected)
            {
                if(Input.GetButtonDown("X"))
                {
                    if(marquageManager.marquageControllers.Count == 0)
                    {
                        multipleAttack = false;
                    }
                    else
                    {
                        multipleAttack = true;
                    }

                    barsCharge.SetActive(true);
                }

                if (Input.GetButton("X"))
                {
                    if(multipleAttack == false)
                    {
                        if (chargeTimer < chargeTime)
                        {
                            chargeTimer += Time.deltaTime;
                            barsCharge.SetActive(true);
                        }
                    }
                    else if (multipleAttack == true)
                    {
                        StartCoroutine(MultiplesVersatilAttack());
                    }
                    
                }

                if(Input.GetButtonUp("X"))
                {
                    if(multipleAttack == false)
                    {
                        if (chargeTimer >= chargeTime)
                        {
                            VersatilAttack(levelMultiplicator[2]);
                        }
                        else if (chargeTimer >= (chargeTime * (1f / 4f)) )
                        {
                            VersatilAttack(levelMultiplicator[1]);
                        }
                        else if (chargeTimer < chargeTime * (1f / 4f))
                        {
                            VersatilAttack(levelMultiplicator[0]);
                        }
                    }
                    else if(multipleAttack == true)
                    {
                        StartCoroutine(MultiplesVersatilAttack());
                    }

                    chargeTimer = 0;

                    barsCharge.SetActive(false);

                }

            }
            else
            {
                chargeTimer = 0;

                barsCharge.SetActive(false);
            }

        }
        else
        {
            timerCooldownVersatilAttack -= Time.deltaTime;
        }

        chargeSpeedMultiplicator = 1 - ((chargeTimer) / (chargeTime + 0.6f));

        barCharge.localScale = new Vector2((chargeTimer) / (chargeTime), barCharge.localScale.y);

        directionAngle = Vector2.Angle(transform.right, direction);

        if (direction.y < 0)
        {
            directionAngle = -directionAngle;
        }

        arrowDirection.rotation = Quaternion.Euler(0, 0, directionAngle);
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
        for(float i = durationBaseAttack; i > 0; i -= Time.deltaTime)
        {
            rb.velocity = -direction.normalized * speedKnockBackBaseAttack * Time.deltaTime;

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

    IEnumerator MultiplesVersatilAttack()
    {
        List<Elements_Controller> copyMarquageControllers = new List<Elements_Controller>();

        for (int i = 0; i <  marquageManager.marquageControllers.Count; i++)
        {
            copyMarquageControllers.Add(marquageManager.marquageControllers[i].GetComponentInParent<Elements_Controller>());
        }

        for (int i = 0; i < copyMarquageControllers.Count; i++)
        {
            StartCoroutine(copyMarquageControllers[i].StunedForSeconds(5f));
        }

        for (int i = 0; i < copyMarquageControllers.Count; i++)
        {
            VersatilAttack(copyMarquageControllers[i].transform, levelMultiplicator[1]);

            MarquageController mC = copyMarquageControllers[i].GetComponentInChildren<MarquageController>();

            if (mC != null)
            {
                Destroy(mC.gameObject);

                marquageManager.marquageControllers.Remove(mC);
            }


            projected = true;

            while (projected == true)
            {
                yield return null;
            }

            projected = true;

            yield return new WaitForSeconds(0.2f);

        }

        projected = false;

        marquageManager.marquageControllers.Clear();
    }

    void VersatilAttack(float levelProjecting)
    {
        Bullet_Versatil_Controller bullet = Instantiate(bulletVersatilAttack, vBullets).GetComponent<Bullet_Versatil_Controller>();

        bullet.transform.position = transform.position;

        //projected = true;
        //rb.velocity = new Vector2(0, 0);

        bullet.player = transform;
        bullet.maxDistance = rangeMaxVersatilAttack;
        bullet.levelProjecting = levelProjecting;

        bullet.rb.velocity = direction.normalized * speedBulletVersatil * Mathf.Min(3f, levelProjecting) * Time.deltaTime;

        timerCooldownVersatilAttack = cooldownVersatilAttack;
    }

    void VersatilAttack(Transform target, float levelProjecting)
    {
        Bullet_Versatil_Controller bullet = Instantiate(bulletVersatilAttack, vBullets).GetComponent<Bullet_Versatil_Controller>();

        bullet.transform.position = transform.position;

        bullet.player = transform;
        bullet.maxDistance = 20;
        bullet.levelProjecting = levelProjecting;

        bullet.rb.velocity = (target.position - transform.position).normalized * (speedBulletVersatil * levelProjecting * 1.5f) * Time.deltaTime;

        timerCooldownVersatilAttack = cooldownVersatilAttack;
    }

    public void StartCanSpringAttack(bool specialAbility, float time)
    {
        StartCoroutine(CanSpringAttackFor(time));
    }

    IEnumerator CanSpringAttackFor(float time)
    {
        canSpringAttack = true;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        canSpringAttack = false;
    }
}
