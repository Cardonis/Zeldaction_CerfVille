using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Main_Controller : MonoBehaviour
{
    public Animator fadeAnimator;

    public GameObject pressX;

    public InventoryBook inventoryBook;

    public SaveDisplay saveDisplay;

    public LifeDisplay lifeDisplay;

    public ParticleSystem damageParticleSystem;

    public ParticleSystem chargeParticleSystem;

    public ParticleSystem walkParticleSystem;

    public TelegraphAttack telegraphAttack;

    public Material outlineMaterial;

    public Sprite arrowLevel2;
    public Sprite arrowLevel3;

    [Range(200f, 1600f)] public float speed;

    [HideInInspector] public int currentLife;
    public int maxLife;

    public int part;

    [HideInInspector] public Collider2D physicCollider;

    [Header("Base Attack")]

    public float durationBaseAttack;
    public float cooldownBaseAttack;
    float timerCooldownBaseAttack;
    public float speedKnockBackBaseAttack;
    public bool canTrack;

    Transform baseAttackCollidersParent;

    MarquePlaceur marquePlaceur;

    public Collider2D[] baseAttackColliders;
    //PlaceHolder pour le  moment, avec l'animation il y en aura pas besoin
    public GameObject[] baseAttackSRs;

    [Header("Versatil Attack")]

    public GameObject bulletVersatilAttack;
    public float speedBulletVersatil;
    public float cooldownVersatilAttack;
    [HideInInspector] public float timerCooldownVersatilAttack;
    public float chargeTime;
    float chargeTimer;
    float chargeTimeLevel2;
    float chargeTimeLevel3;
    bool multipleAttack = false;
    float chargeSpeedMultiplicator;
    public float forceValueVersatilAttack;
    public float rangeMaxVersatilAttack;
    public float marquageDuration;
    [HideInInspector] public float lastAttackVersatilTimer = 1f;
    float lastAttackForce = 1;
    bool charging = false;
    int telegraphedLevel = 1;

    [HideInInspector] public bool canSpringAttack;
    [HideInInspector] public bool canAccelerate;

    public List<float> levelMultiplicator = new List<float>();

    //PlaceHolder pour le  moment, avec l'animation il y en aura pas besoin
    public GameObject barsCharge2;
    public GameObject barsCharge3;
    public Transform barCharge;

    [HideInInspector] public Transform vBullets;

    public Transform arrowDirection;
    public SpriteRenderer arrowBackSR;

    [HideInInspector] public bool projected = false;
    [HideInInspector] public bool stunned = false;

    [HideInInspector] public bool baseAttacking = false;
    [HideInInspector] public Coroutine lastBaseAttack;

    Coroutine lastStunnedFor;

    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public MarquageManager marquageManager;
    public RoomTransitionController confiner;
    public CameraShake cameraShake;

    Vector2 input;
    [HideInInspector] public Vector2 direction;
    float inverseDirectionAngle;

    Vector2 inputAim;
    [HideInInspector] public Vector2 directionAim;
    float directionAimAngle;

    //Variable Animator
    public Animator animator;

    AudioManager audiomanager;

    bool carrying;
    [HideInInspector] public Elements_Controller currentPierre;

    [HideInInspector] public bool invincible = false;

    bool dying = false;

    [HideInInspector] public bool gainingLife = false;

    public Color[] ennemyColorDamages = new Color[5];

    void Start()
    {
        direction = Vector2.down;

        directionAim = Vector2.down;

        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        baseAttackCollidersParent = transform.Find("PColliders").Find("PAttackColliders").Find("PBaseAttackColliders");
        physicCollider = transform.Find("PColliders").Find("PPhysicsCollider").GetComponent<Collider2D>();

        marquePlaceur = baseAttackCollidersParent.GetComponentInChildren<MarquePlaceur>();

        vBullets = GameObject.Find("VBullets").transform;

        marquageManager = GameObject.Find("MarquageManager").GetComponent<MarquageManager>();

        rb = GetComponent<Rigidbody2D>();

        canTrack = true;

        chargeTimeLevel2 = chargeTime / 3f;
        chargeTimeLevel3 = chargeTime;

        currentLife = maxLife;

        for (int i = 0; i < baseAttackSRs.Length; i++)
        {
            baseAttackSRs[i].SetActive(false);
        }

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackColliders[i].enabled = false;
        }

        barsCharge2.SetActive(false);
        barsCharge3.SetActive(false);
        barCharge.gameObject.SetActive(false);

        chargeParticleSystem.loop = false;

        damageParticleSystem.gameObject.SetActive(false);
    }


    void Update()
    {

        if(part == 3)
        {
            arrowBackSR.sprite = arrowLevel3;
        }
        else if(part == 2)
        {
            arrowBackSR.sprite = arrowLevel2;
        }

        if(projected)
        {
            if (rb.velocity.magnitude <= 5)
            {
                rb.velocity = Vector2.zero;
                projected = false;
            }

            Physics2D.IgnoreLayerCollision(13, 14, false);

        }
        else
        {
            Physics2D.IgnoreLayerCollision(13, 14, true);
        }

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputAim = new Vector2(Input.GetAxis("RightHorizontal"), -Input.GetAxis("RightVertical"));

        if (!projected && !stunned)
        {
            if (input.magnitude > 0)
            {
                if (input.magnitude > 1)
                    direction = input.normalized;
                else
                    direction = input;


                if(currentPierre != null)
                {
                    rb.velocity = (direction * speed * chargeSpeedMultiplicator * Time.fixedDeltaTime) * ( 6 - currentPierre.mass) / 6;
                }
                else
                {
                    rb.velocity = direction * speed * chargeSpeedMultiplicator * Time.fixedDeltaTime;
                }

            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }

            if (inputAim.magnitude > 0)
            {
                directionAim = inputAim;
            }

        }

        if (currentPierre != null)
        {
            if (currentPierre.projected == true || projected == true || stunned == true)
            {
                Physics2D.IgnoreCollision(physicCollider, currentPierre.GetComponentInChildren<Collider2D>(), false);
                currentPierre = null;
            }
        }

        if (currentPierre != null)
        {
            directionAim = (currentPierre.transform.position - transform.position).normalized;

            currentPierre.transform.position = (Vector2)transform.position + directionAim * 0.8f;

            Physics2D.IgnoreCollision(physicCollider, currentPierre.GetComponentInChildren<Collider2D>(), true);
        }

        if (timerCooldownBaseAttack < 0)
        {
            if ( Input.GetButtonDown("LB") && ((!projected && !stunned) || canSpringAttack) )
            {
                lastBaseAttack = StartCoroutine(BaseAttack());
                audiomanager.Play("Coup_de_vent");
            }
        }
        else
        {
            timerCooldownBaseAttack -= Time.deltaTime;
        }

        if(lastAttackVersatilTimer < 0.5f)
        {
            lastAttackVersatilTimer += Time.deltaTime;
        }

        if (timerCooldownVersatilAttack < 0 && part >= 2)
        {
            if (marquageManager.marquageControllers.Count != 0)
            {
                pressX.SetActive(true);
            }

            if (!projected && !stunned)
            {

                if(Input.GetButtonDown("RB") || Input.GetButtonDown("X"))
                {
                    if(part == 3)
                    {
                        chargeTime = chargeTimeLevel3;
                        //barsCharge3.SetActive(true);
                    }
                    else
                    {
                        chargeTime = chargeTimeLevel2;
                        //barsCharge2.SetActive(true);
                    }

                    //barCharge.gameObject.SetActive(true);

                    if (Input.GetButtonDown("RB") && part > 1)
                    {
                        multipleAttack = false;

                        chargeParticleSystem.loop = true;
                        chargeParticleSystem.Play();

                        charging = true;

                        audiomanager.Play("Capa_charge");
                    }
                    else if(marquageManager.marquageControllers.Count != 0 && Input.GetButtonDown("X") && part > 1)
                    {
                        multipleAttack = true;

                        chargeParticleSystem.loop = true;
                        chargeParticleSystem.Play();

                        charging = true;

                        audiomanager.Play("Capa_charge");
                    }
                    else
                    {
                        barsCharge3.SetActive(false);
                        barsCharge2.SetActive(false);
                        barCharge.gameObject.SetActive(false);

                        charging = false;
                    }
                }

                if (charging)
                {
                    animator.SetBool("IsChargingCV", true);

                    if(multipleAttack == false)
                    {
                        if (chargeTimer < chargeTime)
                        {
                            chargeTimer += Time.deltaTime;
                        }
                    }
                    else if (multipleAttack == true)
                    {
                        marquageManager.ResetTimer(marquageDuration);

                        if (chargeTimer < chargeTime)
                        {
                            chargeTimer += Time.deltaTime;
                        }
                    }

                    if (part == 3)
                    {
                        if (chargeTimer >= chargeTime)
                        {
                            if (telegraphedLevel == 2)
                                telegraphAttack.lastLight = StartCoroutine(telegraphAttack.LitLight(50f, telegraphAttack.baseIntensity * 2f));

                            telegraphedLevel = 3;
                        }
                        else if (chargeTimer >= (chargeTime * (1f / 3f)))
                        {
                            if (telegraphedLevel == 1)
                                telegraphAttack.lastLight = StartCoroutine(telegraphAttack.LitLight(50f, telegraphAttack.baseIntensity / 10f));

                            telegraphedLevel = 2;
                        }
                    }
                    else if (part == 2)
                    {
                        if (chargeTimer >= chargeTime)
                        {
                            if (telegraphedLevel == 1)
                                telegraphAttack.lastLight = StartCoroutine(telegraphAttack.LitLight(50f, telegraphAttack.baseIntensity));

                            telegraphedLevel = 2;
                        }
                    }

                    if (part == 3)
                    {
                        //barsCharge3.SetActive(true);
                    }
                    else
                    {
                        //barsCharge2.SetActive(true);
                    }

                    pressX.SetActive(false);

                    //barCharge.gameObject.SetActive(true);
                }

                if((Input.GetButtonUp("RB") || Input.GetButtonUp("X")) && charging)
                {
                    animator.SetBool("IsChargingCV", false);
                    audiomanager.Stop("Capa_charge");

                    if (multipleAttack == false)
                    {
                        if(part == 3)
                        {
                            if (chargeTimer >= chargeTime)
                            {
                                VersatilAttack(levelMultiplicator[2]);
                            }
                            else if (chargeTimer >= (chargeTime * (1f / 3f)))
                            {
                                VersatilAttack(levelMultiplicator[1]);
                            }
                            else if (chargeTimer < chargeTime * (1f / 3f))
                            {
                                VersatilAttack(levelMultiplicator[0]);
                            }
                        }
                        else if(part == 2)
                        {
                            if (chargeTimer >= chargeTime)
                            {
                                VersatilAttack(levelMultiplicator[1]);
                            }
                            else
                            {
                                VersatilAttack(levelMultiplicator[0]);
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

                    pressX.SetActive(false);

                    charging = false;
                    chargeParticleSystem.loop = false;

                    if(telegraphAttack.lastLight != null)
                    StopCoroutine(telegraphAttack.lastLight);

                    StartCoroutine(telegraphAttack.UnlitLight(50f));

                    telegraphedLevel = 1;

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

        if (confiner.activeRoom != null)
        {
            List<Elements_Controller> pierresActivesRoom = new List<Elements_Controller>();

            if (part >= 2)
            {
                for (int i = 0; i < confiner.activeRoom.ennemies.Count; i++)
                {
                    confiner.activeRoom.ennemies[i].outlineController.outLining = false;
                    if (confiner.activeRoom.ennemies[i].dead == false)
                        pierresActivesRoom.Add(confiner.activeRoom.ennemies[i]);
                }
            }

            for (int i = 0; i < confiner.activeRoom.objectsToReset.Count; i++)
            {
                confiner.activeRoom.objectsToReset[i].outlineController.outLining = false;
                pierresActivesRoom.Add(confiner.activeRoom.objectsToReset[i]);
            }

            for (int i = 0; i < confiner.activeRoom.objectsToDestroy.Count; i++)
            {
                confiner.activeRoom.objectsToDestroy[i].outlineController.outLining = false;
                pierresActivesRoom.Add(confiner.activeRoom.objectsToDestroy[i]);
            }

            //pierresActivesRoom = confiner.activeRoom.objectsToReset + confiner.activeRoom.objectsToDestroy;

            Elements_Controller nearestPierre = null;

            for (int i = 0; i < pierresActivesRoom.Count; i++)
            {
                if (Vector2.Distance(transform.position, pierresActivesRoom[i].transform.position) < 2f && pierresActivesRoom[i].isTractable)
                {
                    if (nearestPierre != null)
                    {
                        if (Vector2.Distance(transform.position, nearestPierre.transform.position) > Vector2.Distance(transform.position, pierresActivesRoom[i].transform.position))
                        {
                            nearestPierre = pierresActivesRoom[i];
                        }
                    }
                    else
                    {
                        nearestPierre = pierresActivesRoom[i];
                    }

                }
            }

            if (nearestPierre != null)
            {
                if (currentPierre == null)
                {
                    nearestPierre.outlineController.outLining = true;

                    if (Input.GetButtonDown("X") && canTrack == true)
                    {
                        currentPierre = nearestPierre;
                    }
                }
                else
                {
                    if (Input.GetButtonDown("X") && canTrack == true)
                    {
                        Physics2D.IgnoreCollision(physicCollider, currentPierre.GetComponentInChildren<Collider2D>(), false);
                        currentPierre = null;
                    }
                }
            }
        }

        chargeSpeedMultiplicator = 1 - ((chargeTimer) / (chargeTime + 0.6f));

        barCharge.localScale = new Vector2(((chargeTimer) / (chargeTime)) * 1.5f, barCharge.localScale.y );

        inverseDirectionAngle = Vector2.Angle(transform.right, -direction);

        if (direction.y > 0)
        {
            inverseDirectionAngle = -inverseDirectionAngle;
        }

        walkParticleSystem.transform.rotation = Quaternion.Euler(0, 0, inverseDirectionAngle);

        directionAimAngle = Vector2.Angle(transform.right, directionAim);

        if (directionAim.y < 0)
        {
            directionAimAngle = -directionAimAngle;
        }
        
        arrowDirection.rotation = Quaternion.Euler(0, 0, directionAimAngle);

        if(part >= 2)
        {
            arrowBackSR.enabled = true;
        }
        else
        {
            arrowBackSR.enabled = false;
        }

        //Animator Main Region
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
            baseAttackSRs[i].SetActive(true);

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
            rb.velocity = -directionAim.normalized * speedKnockBackBaseAttack * Time.deltaTime;

            yield return null;
        }

        timerCooldownBaseAttack = cooldownBaseAttack;

        for (int i = 0; i < baseAttackSRs.Length; i++)
        {
            baseAttackSRs[i].SetActive(false);
        }

        stunned = false;

        baseAttackCollidersParent.rotation = Quaternion.identity;

        for (int i = 0; i < baseAttackColliders.Length; i++)
        {
            baseAttackColliders[i].enabled = false;
        }

        baseAttacking = false;
    }

    //Animator l'attaque de base
    #region Animator AttaqueB
    void AnimatorAttaqueB()
    {

        if (baseAttacking)
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
            if (copyMarquageControllers[i].gameObject.activeSelf == true) 
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

            yield return new WaitForSeconds(0.1f);

        }

        canAccelerate = false;

        stunned = false;
    }

    void VersatilAttack(float levelProjecting)
    {
        Bullet_Versatil_Controller bullet = Instantiate(bulletVersatilAttack, vBullets).GetComponent<Bullet_Versatil_Controller>();
        audiomanager.Play("Capa_Liane");

        bullet.transform.position = transform.position;

        //projected = true;
        //rb.velocity = new Vector2(0, 0);

        bullet.player = this;
        bullet.maxDistance = (rangeMaxVersatilAttack / 4f) + (rangeMaxVersatilAttack * 3f / 4f) / levelProjecting;
        bullet.levelProjecting = levelProjecting;

        bullet.rb.velocity = directionAim.normalized * speedBulletVersatil / Mathf.Min(3f, levelProjecting) * Time.fixedDeltaTime;

        if(lastAttackVersatilTimer <= 0.5f)
        {
            bullet.bonusLevelProjecting = ( Mathf.Min((lastAttackForce * 2), 4f) - 1);
        }
        else
        {
            bullet.bonusLevelProjecting = 0;
        }

        if(levelProjecting + bullet.bonusLevelProjecting == 1)
        {
            bullet.GetComponent<LineRenderer>().SetWidth(0.20f, 0.10f);
        }
        else if (levelProjecting + bullet.bonusLevelProjecting == 2)
        {
            bullet.GetComponent<LineRenderer>().SetWidth(0.40f, 0.20f);
        }
        else if (levelProjecting + bullet.bonusLevelProjecting == 4)
        {
            bullet.GetComponent<LineRenderer>().SetWidth(0.80f, 0.40f);
        }

        lastAttackForce = levelProjecting + bullet.bonusLevelProjecting;

        timerCooldownVersatilAttack = cooldownVersatilAttack;
        animator.SetTrigger("UsesCapaV");
    }

    void VersatilAttack(Transform target, float levelProjecting)
    {
        Bullet_Versatil_Controller bullet = Instantiate(bulletVersatilAttack, vBullets).GetComponent<Bullet_Versatil_Controller>();
        audiomanager.Play("Capa_Liane");
        bullet.transform.position = target.transform.position;

        bullet.player = this;
        bullet.maxDistance = 20;
        bullet.levelProjecting = levelProjecting;

        if(levelProjecting == 1)
        {
            bullet.GetComponent<LineRenderer>().SetWidth(0.20f, 0.10f);
        }
        else if (levelProjecting == 2)
        {
            bullet.GetComponent<LineRenderer>().SetWidth(0.40f, 0.20f);
        }
        else if (levelProjecting == 4)
        {
            bullet.GetComponent<LineRenderer>().SetWidth(0.80f, 0.40f);
        }

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
        StartCoroutine(InvicibilityFrame());

        currentLife -= damageTaken;
        audiomanager.Play("Player_take_damage");

        damageParticleSystem.gameObject.SetActive(true);

        damageParticleSystem.Play();

        //Stop charge 
        pressX.SetActive(false);

        charging = false;
        chargeParticleSystem.loop = false;

        StartCoroutine( telegraphAttack.UnlitLight(50f) );

        telegraphedLevel = 1;

        chargeTimer = 0;

        barsCharge2.SetActive(false);
        barsCharge3.SetActive(false);
        barCharge.gameObject.SetActive(false);
        //End Stop charge

        //Animator
        animator.SetTrigger("IsHurt");

        GetComponentInChildren<SpriteRenderer>().color = Color.red;

        StartCoroutine(cameraShake.CameraShakeFor(0.2f, 0.2f, 3));

        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        GetComponentInChildren<SpriteRenderer>().color = Color.white;

        if (currentLife <= 0 && !dying)
        {
            StartCoroutine( Die() );
        }



    }

    public IEnumerator GainLife(int lifeToRegain)
    {
        gainingLife = true;

        for(int i = 0; i < lifeToRegain; i++)
        {
            if (currentLife == maxLife)
            {
                yield return new WaitForSeconds(0.5f);

                break;
            }

            currentLife += 1;

            yield return new WaitForSeconds(0.5f);
        }

        gainingLife = false;
    }

    IEnumerator InvicibilityFrame()
    {
        invincible = true;

        for (float i = 0.8f; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        invincible = false;
    }

    IEnumerator Die()
    {
        stunned = true;
        dying = true;

        MusicManager.InBattle = false;
        MusicManager.EnemyInBattle = 0;

        fadeAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1.5f);

        LoadPlayer();

        confiner.cmVcam.m_Lens.OrthographicSize = 7.5f;
        confiner.transform.localScale = new Vector3(1, 1, 1);

        GameObject[] rs = GameObject.FindGameObjectsWithTag("Room");

        foreach (GameObject r in rs)
        {
            r.GetComponent<RoomController>().FullReset();
        }

        currentLife = maxLife;

        fadeAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1f);

        stunned = false;
        dying = false;

        yield break;
    }

    public void TakeForce(Vector2 direction, float forceValue)
    {
        projected = true;
        audiomanager.Play("Capa_Lance");
        rb.velocity = new Vector2(0, 0);

        rb.AddForce(direction * forceValue, ForceMode2D.Impulse);

    }

    public void SavePlayer()
    {
        saveDisplay.StartCoroutine(saveDisplay.DisplayFor(2f));

        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        part = data.part;
        maxLife = data.maxHealth;

        Vector2 position;
        position.x = data.position[0];
        position.y = data.position[1];

        transform.position = position;

        Vector2 confinerPosition;
        confinerPosition.x = data.confinerPosition[0];
        confinerPosition.y = data.confinerPosition[1];

        confiner.transform.position = confinerPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller ec = collision.transform.GetComponentInParent<Elements_Controller>();

        if ((collision.transform.tag == "Wall" || ec != null) && projected == true)
        {
            if(invincible == false)
            StartCoroutine( TakeDamage(1) );
        }

        if(ec != null)
        {
            if (ec.projected == true && ec.playerProjected == false)
            {
                if(invincible == false)
                StartCoroutine( TakeDamage(1) );
            }
        }
    }

    
}
