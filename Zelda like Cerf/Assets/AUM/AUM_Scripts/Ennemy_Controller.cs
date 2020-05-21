using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ennemy_Controller : Elements_Controller
{
    public ParticleSystem damageParticleSystem;

    [Header("BaseStats")]
    public float pv;
    [HideInInspector] public float initialLife;
    public float speed;
    public float detectionAngle;
    public float detectionDistance;
    public bool playerDetected;
    [HideInInspector] public bool marqued;
    [HideInInspector] public float playerAngle;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool attacking = false;
    [HideInInspector] public Coroutine lastAttack;

    [HideInInspector] public bool dead = false;

    public LayerMask detection;

    public List<Ennemy_Controller> ennemyControllersList;
    public List<Caisse_Controller> caisseControllersList;

    float cooldownWandering;

    [Header("Wandering")]
    [Range(0.5f, 3f)] public float minWanderingDistance;
    [Range(2f, 6f)] public float maxWanderingDistance;
    bool canWander;
    public Vector2 target;
    public bool canMove = true;
    float wanderingTime;

    [HideInInspector] public bool invincible = false;

    public Collider2D col;

    public TelegraphAttack telegraphAttack;

    private bool PlayerWasDetected;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Elements_Controller ec = collision.transform.GetComponent<Elements_Controller>();

        if (projected == true && levelProjected >= 0.5f && velocityBeforeImpact.magnitude > 5f)
        {

            if (collision.transform.tag == "Wall" || collision.transform.tag == "Ronce")
            {
                if(invincible == false)
                    StartCoroutine(TakeDamage(levelProjected, velocityBeforeImpactAngle));
            }

            if (ec != null)
            {
                if (invincible == false)
                    StartCoroutine(TakeDamage(levelProjected, velocityBeforeImpactAngle));
                
            }
        }

        if(ec != null && ec.velocityBeforeImpact.magnitude > 5f)
        {
            if(ec.projected && ec.levelProjected >= 0.5f)
            {
                TakeForce(transform.position - ec.transform.position, 15f ,ec.levelProjected);
                if (invincible == false)
                    StartCoroutine(TakeDamage(ec.levelProjected, velocityBeforeImpactAngle));
            }
        }        

    }

    public void StartTakeDamage(float damageTaken, float damageDirectionAngle)
    {
        StartCoroutine(TakeDamage(damageTaken, damageDirectionAngle));
    }

    IEnumerator TakeDamage(float damageTaken, float damageDirectionAngle)
    {
        StartCoroutine(InvicibilityFrame());

        pv -= damageTaken;

        damageParticleSystem.transform.localEulerAngles = rb.velocity;

        damageParticleSystem.transform.rotation = Quaternion.Euler(0, 0, damageDirectionAngle);


        switch(damageTaken)
        {
            case 0.5f:
                damageParticleSystem.startColor = player.ennemyColorDamages[0];
                damageParticleSystem.startSpeed = 5;
                damageParticleSystem.startSize = 0.05f;
                break;

            case 1:
                damageParticleSystem.startColor = player.ennemyColorDamages[1];
                damageParticleSystem.startSpeed = 10;
                damageParticleSystem.startSize = 0.1f;
                break;

            case 2:
                damageParticleSystem.startColor = player.ennemyColorDamages[2];
                damageParticleSystem.startSpeed = 20;
                damageParticleSystem.startSize = 0.2f;
                break;

            case 4:
                damageParticleSystem.startColor = player.ennemyColorDamages[3];
                damageParticleSystem.startSpeed = 30;
                damageParticleSystem.startSize = 0.3f;
                break;

            case 8:
                damageParticleSystem.startColor = player.ennemyColorDamages[4];
                damageParticleSystem.startSpeed = 40;
                damageParticleSystem.startSize = 0.4f;
                break;
        }

        damageParticleSystem.Play();

        if (player.cameraShake.shaking == false)
            player.cameraShake.lastCameraShake = player.cameraShake.StartCoroutine(player.cameraShake.CameraShakeFor(0.1f, 0.1f,damageTaken));

        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);

        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {

            yield return null;
        }

        GetComponentInChildren<SpriteRenderer>().color = Color.white;

        if (pv <= 0)
        {
            Death();
        }
    }

    IEnumerator InvicibilityFrame()
    {
        invincible = true;

        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        invincible = false;
    }

    public void Death()
    {
        MusicManager.EnemyInBattle -= 1;
        PlayerWasDetected = false;

        if(GetComponentInChildren<Bullet_Versatil_Controller>() != null)
        {
            player.GetComponent<Player_Main_Controller>().stunned = false;

            StopTakeForce();
            Destroy(GetComponentInChildren<Bullet_Versatil_Controller>().gameObject);
        }

        AudioSource[] sourcesToDestroy = GetComponents<AudioSource>();

        foreach (AudioSource sourceToDestroy in sourcesToDestroy)
        {
            Destroy(sourceToDestroy);
        }

        MarquageController mC = GetComponentInChildren<MarquageController>();

        if (mC != null)
        {
            Destroy(mC.gameObject);

            marquageManager.marquageControllers.Remove(mC);
        }

        canMove = true;
        dead = true;

        gameObject.SetActive(false);
    }

    public abstract IEnumerator Attack1();

    public void Wandering()
    {
        if (canMove == true)
        {
            direction = (target - (Vector2)transform.position).normalized;
        }
        else
        {
            direction = Vector2.zero;
            rb.velocity = Vector2.zero;
        }

        if (wanderingTime >= 0)
        {
            wanderingTime -= Time.deltaTime;
        }

        Detection();

        if (Vector2.Distance(target, transform.position) <= 0.1f || wanderingTime < 0)
        {
            if (canWander == false)
            {
                cooldownWandering = Random.Range(1f, 2f);
                canWander = true;
                canMove = false;
            }
            else
            {

                cooldownWandering -= Time.deltaTime;

                if (cooldownWandering <= 0)
                {
                    WanderingNewDirection();
                    wanderingTime = Random.Range(1f, 2f);
                }
            }
        }
    }

    public void WanderingNewDirection()
    {
        target = (new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized * Random.Range(minWanderingDistance, maxWanderingDistance)) + (Vector2)transform.position;
        canWander = false;
        canMove = true;
    }

    public void Detection()
    {
        playerAngle = Vector2.Angle(direction, player.transform.position - transform.position);


        if (playerAngle <= detectionAngle)
        {
            col.enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position,Mathf.Infinity , detection);
            col.enabled = true;

            if(hit.collider.attachedRigidbody != null)
            {
                if(hit.distance <= detectionDistance)
                    if (hit.collider.attachedRigidbody.transform.tag == "Player")
                    {
                        Ennemy_Controller[] ennemy_Controllers = transform.parent.GetComponentsInChildren<Ennemy_Controller>();

                        foreach(Ennemy_Controller ennemy_Controller in ennemy_Controllers)
                        {
                            ennemy_Controller.playerDetected = true;
                            MusicManager.InBattle = true;
                            MusicManager.EnemyInBattle += 1;
                        }

                        playerDetected = true;
                        canMove = true;
                    }
            }
            
        }

    }




}
