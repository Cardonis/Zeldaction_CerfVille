using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Main_Controller : MonoBehaviour
{
    [Range(100.0f, 400.0f)] public float speed;

    public int life;

    Text lifeText;

    public int part;

    [HideInInspector] public Collider2D physicCollider;

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
    float chargeTimeLevel2;
    float chargeTimeLevel3;
    bool multipleAttack = false;
    float chargeSpeedMultiplicator;
    public float forceValueVersatilAttack;
    public float rangeMaxVersatilAttack;
    public float marquageDuration;

    [HideInInspector] public bool canSpringAttack;
    [HideInInspector] public bool canAccelerate;

    public List<float> levelMultiplicator = new List<float>();

    //PlaceHolder pour le  moment, avec l'animation il y en aura pas besoin
    public GameObject barsCharge2;
    public GameObject barsCharge3;
    public Transform barCharge;

    [HideInInspector] public Transform vBullets;

    Transform arrowDirection;

    [HideInInspector] public bool projected = false;
    [HideInInspector] public bool stunned = false;

    [HideInInspector] public bool baseAttacking = false;
    [HideInInspector] public Coroutine lastBaseAttack;

    Coroutine lastStunnedFor;

    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public MarquageManager marquageManager;
    [HideInInspector] public RoomTransitionController confiner;

    Vector2 input;
    [HideInInspector] public Vector2 direction;
    float directionAngle;

    Vector2 inputAim;
    [HideInInspector] public Vector2 directionAim;
    float directionAimAngle;

    //Variable Animator
    [HideInInspector] public Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        baseAttackCollidersParent = transform.Find("PColliders").Find("PAttackColliders").Find("PBaseAttackColliders");
        physicCollider = transform.Find("PColliders").Find("PPhysicsCollider").GetComponent<Collider2D>();

        arrowDirection = transform.Find("Arrow");

        vBullets = GameObject.Find("VBullets").transform;

        lifeText = GameObject.Find("LifeText").GetComponent<Text>();

        confiner = GameObject.Find("CameraConfiner").GetComponent<RoomTransitionController>();
        marquageManager = GameObject.Find("MarquageManager").GetComponent<MarquageManager>();

        rb = GetComponent<Rigidbody2D>();

        chargeTimeLevel2 = chargeTime / 3f;
        chargeTimeLevel3 = chargeTime;

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackSRs[i].enabled = false;
        }

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackColliders[i].enabled = false;
        }

        barsCharge2.SetActive(false);
        barsCharge3.SetActive(false);
        barCharge.gameObject.SetActive(false);
    }


    void Update()
    {
        lifeText.text = "Life : " + life;

        if(projected)
        {
            if (rb.velocity.magnitude <= 5)
            {
                rb.velocity = Vector2.zero;
                projected = false;
            }
        }

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputAim = new Vector2(Input.GetAxis("RightHorizontal"), -Input.GetAxis("RightVertical"));

        if (!projected && !stunned)
        {
            if(inputAim.magnitude > 0)
            {
                directionAim = inputAim;
            }

            if (input.magnitude > 0)
            {
                direction = input;
                FindObjectOfType<AudioManager>().Play("Deplacement");
                FindObjectOfType<AudioManager>().AlreadyPlay("Deplacement");
                rb.velocity = direction * speed * chargeSpeedMultiplicator * Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
                FindObjectOfType<AudioManager>().Stop("Deplacement");
                FindObjectOfType<AudioManager>().RePlay("Deplacement");
            }
        
        }

        if (timerCooldownBaseAttack < 0)
        {
            if ( Input.GetButtonDown("LB") && ((!projected && !stunned) || canSpringAttack) )
            {
                lastBaseAttack = StartCoroutine(BaseAttack());
                FindObjectOfType<AudioManager>().Play("Coup_de_vent");
            }
        }
        else
        {
            timerCooldownBaseAttack -= Time.deltaTime;
        }

        if (timerCooldownVersatilAttack < 0 && part >= 2)
        {
            if(!projected && !stunned)
            {
                if(Input.GetButtonDown("RB"))
                {
                    if(part == 3)
                    {
                        chargeTime = chargeTimeLevel3;
                        barsCharge3.SetActive(true);
                    }
                    else
                    {
                        chargeTime = chargeTimeLevel2;
                        barsCharge2.SetActive(true);
                    }

                    if(marquageManager.marquageControllers.Count == 0)
                    {
                        multipleAttack = false;
                    }
                    else
                    {
                        multipleAttack = true;
                    }
                    FindObjectOfType<AudioManager>().Play("Capa_charge");
                    barCharge.gameObject.SetActive(true);
                }

                if (Input.GetButton("RB"))
                {
                    if(multipleAttack == false)
                    {
                        if (chargeTimer < chargeTime)
                        {
                            chargeTimer += Time.deltaTime;
                        }
                    }
                    else if (multipleAttack == true)
                    {
                        if (chargeTimer < chargeTime)
                        {
                            chargeTimer += Time.deltaTime;
                        }
                    }

                    if (part == 3)
                    {
                        barsCharge3.SetActive(true);
                    }
                    else
                    {
                        barsCharge2.SetActive(true);
                    }

                    barCharge.gameObject.SetActive(true);
                }

                if(Input.GetButtonUp("RB"))
                {
                    FindObjectOfType<AudioManager>().Stop("Capa_charge");

                    if (multipleAttack == false)
                    {
                        if(part == 3)
                        {
                            if (chargeTimer >= chargeTime)
                            {
                                VersatilAttack(levelMultiplicator[2]);
                                FindObjectOfType<AudioManager>().Play("Capa_simple_niv3");
                            }
                            else if (chargeTimer >= (chargeTime * (1f / 3f)))
                            {
                                VersatilAttack(levelMultiplicator[1]);
                                FindObjectOfType<AudioManager>().Play("Capa_simple_niv2");
                            }
                            else if (chargeTimer < chargeTime * (1f / 3f))
                            {
                                VersatilAttack(levelMultiplicator[0]);
                                FindObjectOfType<AudioManager>().Play("Capa_simple_niv1");
                            }
                        }
                        else if(part == 2)
                        {
                            if (chargeTimer >= chargeTime)
                            {
                                VersatilAttack(levelMultiplicator[1]);
                                FindObjectOfType<AudioManager>().Play("Capa_simple_niv2");
                            }
                            else
                            {
                                VersatilAttack(levelMultiplicator[0]);
                                FindObjectOfType<AudioManager>().Play("Capa_simple_niv1");
                            }
                        }
                        
                    }
                    else if(multipleAttack == true)
                    {
                        if (part == 3)
                        {
                            if (chargeTimer >= chargeTime)
                            {
                                StartCoroutine(MultiplesVersatilAttack(levelMultiplicator[2]));
                            }
                            else if (chargeTimer >= (chargeTime * (1f / 3f)))
                            {
                                StartCoroutine(MultiplesVersatilAttack(levelMultiplicator[1]));
                            }
                            else if (chargeTimer < chargeTime * (1f / 3f))
                            {
                                StartCoroutine(MultiplesVersatilAttack(levelMultiplicator[0]));
                            }
                        }
                        else if (part == 2)
                        {
                            if (chargeTimer >= chargeTime)
                            {
                                StartCoroutine(MultiplesVersatilAttack(levelMultiplicator[1]));
                            }
                            else
                            {
                                StartCoroutine(MultiplesVersatilAttack(levelMultiplicator[0]));
                            }
                        }
                    }

                    chargeTimer = 0;

                    barsCharge2.SetActive(false);
                    barsCharge3.SetActive(false);
                    barCharge.gameObject.SetActive(false);

                }

            }
            else
            {
                chargeTimer = 0;

                barsCharge2.SetActive(false);
                barsCharge3.SetActive(false);
                barCharge.gameObject.SetActive(false);
            }

        }
        else
        {
            timerCooldownVersatilAttack -= Time.deltaTime;
        }

        chargeSpeedMultiplicator = 1 - ((chargeTimer) / (chargeTime + 0.6f));

        barCharge.localScale = new Vector2(((chargeTimer) / (chargeTime)) * 1.5f, barCharge.localScale.y );

        directionAngle = Vector2.Angle(transform.right, direction);

        if (direction.y < 0)
        {
            directionAngle = -directionAngle;
        }

        directionAimAngle = Vector2.Angle(transform.right, directionAim);

        if(directionAim.y < 0)
        {
            directionAimAngle = -directionAimAngle;
        }

        arrowDirection.rotation = Quaternion.Euler(0, 0, directionAimAngle);

        //Animator pour les déplacements
        #region Animator
        if (input.magnitude > 0)
        { 
            animator.SetBool("IsMoving", true);
        }
        else 
        {
            animator.SetBool("IsMoving", false);
        }

        if (0.8 < input.magnitude)
        {
            animator.SetBool("IsRunning", true);
        }
        else 
        {
            animator.SetBool("IsRunning", false);
        }

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        animator.SetFloat("HorizontalAim", directionAim.x);
        animator.SetFloat("VerticalAim", directionAim.y);

        AnimatorAttaqueB();

        #endregion Animator
    }

    IEnumerator BaseAttack()
    {
        baseAttacking = true;
        

        if (lastStunnedFor != null)
            StopCoroutine(lastStunnedFor);
        lastStunnedFor = StartCoroutine(StunnedFor(durationBaseAttack + 0.05f));

        baseAttackCollidersParent.rotation = Quaternion.Euler(0, 0, directionAimAngle);

        //Activation des Sprite Renderes
        for (int i = 0; i < baseAttackSRs.Length; i++)
        {
            if(i != 0)
            baseAttackSRs[i].enabled = true;
            else
            {
                if(part >= 2)
                {
                    baseAttackSRs[i].enabled = true;
                }
            }
        }

        //Activation de l'hitbox du sweetspot
        if(part >= 2)
        baseAttackColliders[0].enabled = true;

        yield return new WaitForSeconds(0.05f);

        //Activation de toutes les hitboxs
        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            if (i != 0)
                baseAttackColliders[i].enabled = true;
            else
            {
                if (part >= 2)
                {
                    baseAttackColliders[i].enabled = true;
                }
            }
        }

        //Attack duration + knock back
        for(float i = durationBaseAttack; i > 0; i -= Time.deltaTime)
        {
            rb.velocity = -directionAim.normalized * speedKnockBackBaseAttack * Time.deltaTime;

            yield return null;
        }

        timerCooldownBaseAttack = cooldownBaseAttack;

        for (int i = 0; i < baseAttackSRs.Length; i++)
        {
            if (i != 0)
                baseAttackSRs[i].enabled = false;
            else
            {
                if (part >= 2)
                {
                    baseAttackSRs[i].enabled = false;
                }
            }
        }

        stunned = false;

        baseAttackCollidersParent.rotation = Quaternion.identity;

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            if (i != 0)
                baseAttackColliders[i].enabled = false;
            else
            {
                if (part >= 2)
                {
                    baseAttackColliders[i].enabled = false;
                }
            }
        }

        baseAttacking = false;
    }

    //Animator l'attaque de base
    #region Animator AttaqueB
    void AnimatorAttaqueB()
    {

        if (baseAttacking == true)
        {
            animator.SetTrigger("IsBaseAttacking");
        }
    }
    

#endregion Animator AttaqueB

IEnumerator MultiplesVersatilAttack(float levelProjecting)
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
            canAccelerate = true;

            VersatilAttack(copyMarquageControllers[i].transform, levelProjecting);

            MarquageController mC = copyMarquageControllers[i].GetComponentInChildren<MarquageController>();

            if (mC != null)
            {
                Destroy(mC.gameObject);

                marquageManager.marquageControllers.Remove(mC);
            }

            yield return new WaitForSeconds(0.4f);

        }

        canAccelerate = false;

        stunned = false;
    }

    void VersatilAttack(float levelProjecting)
    {
        Bullet_Versatil_Controller bullet = Instantiate(bulletVersatilAttack, vBullets).GetComponent<Bullet_Versatil_Controller>();

        bullet.transform.position = transform.position;

        //projected = true;
        //rb.velocity = new Vector2(0, 0);

        bullet.player = transform;
        bullet.maxDistance = (rangeMaxVersatilAttack / 4f) + (rangeMaxVersatilAttack * 3f / 4f) / levelProjecting;
        bullet.levelProjecting = levelProjecting;

        bullet.rb.velocity = directionAim.normalized * speedBulletVersatil / Mathf.Min(3f, levelProjecting) * Time.fixedDeltaTime;

        timerCooldownVersatilAttack = cooldownVersatilAttack;
    }

    void VersatilAttack(Transform target, float levelProjecting)
    {
        Bullet_Versatil_Controller bullet = Instantiate(bulletVersatilAttack, vBullets).GetComponent<Bullet_Versatil_Controller>();

        bullet.transform.position = transform.position;

        bullet.player = transform;
        bullet.maxDistance = 20;
        bullet.levelProjecting = levelProjecting;

        bullet.rb.velocity = (target.position - transform.position).normalized * speedBulletVersatil / Mathf.Min(3f, levelProjecting) * Time.fixedDeltaTime;

        timerCooldownVersatilAttack = cooldownVersatilAttack;
    }

    public void StartCanSpringAttack(float time)
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

    public IEnumerator StunnedFor(float time)
    {
        stunned = true;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        stunned = false;
    }

    public IEnumerator TakeDamage(int damageTaken)
    {
        life -= damageTaken;
        FindObjectOfType<AudioManager>().Play("Player_take_damage");
        Color baseColor = GetComponentInChildren<SpriteRenderer>().material.color;

        GetComponentInChildren<SpriteRenderer>().material.SetColor("_BaseColor", Color.red);

        for (float i = 0.1f; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        GetComponentInChildren<SpriteRenderer>().material.color = baseColor;

        if (life <= 0)
        {
            StartCoroutine( Die() );
        }
    }

    IEnumerator Die()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        yield break;
    }

    public void TakeForce(Vector2 direction, float forceValue)
    {
        projected = true;

        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction * forceValue, ForceMode2D.Impulse);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller ec = collision.transform.GetComponentInParent<Elements_Controller>();

        if ((collision.transform.tag == "Wall" || ec != null) && projected == true)
        {
            StartCoroutine( TakeDamage(1) );
        }

        if(ec != null)
        {
            if (ec.projected == true && ec.playerProjected == false)
            {
                StartCoroutine( TakeDamage(1) );
            }
        }
    }

    
}
