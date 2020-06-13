using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class BossBehavior : Ennemy_Controller
{
    //public EndGameInitiator endGameInitiator;
    public GameObject endCinematic;

    Vector2 directionPattern;
    public int phase = 1;
    float timerCooldownPattern;
    float debugTimerCooldownPattern = 1.5f;
    public RoomController bossRoom;
    public Transform centerTeleport;
    float missingLife;

    float[] cooldownsPatterns = { 1f, 0.5f, 1.5f, 1.5f, 0.5f, 1.5f };

    public GameObject[] phasesGlobalLightChangers;
    GameObject currentGlobalLightChanger;

    [Header("Dash Attack")]
    [Header("Patterns")]
    
    public Collider2D dashAttackCollider;
    public Collider2D physicCollider;

    [Header("Rock Throw")]

    public List<Caisse_Controller> caissesToSpawns;
    Elements_Controller currentElement;
    Collider2D currentElementCol;
    Bullet_Versatil_Controller currentElementBullet;
    Transform pierresParent;
    bool launched;

    [Header("Spectral Liane")]

    public GameObject bulletAttack1;
    float forceSpectralLiane = 1000f;
    float forceSpectralLianePlayer = 1000f;

    [Header("Minions")]

    public List<GameObject> ennemiesToSpawn;
    public Transform parentEnnemies;
    public List<Transform> teleportPoints;
    public List<Transform> spawnPoints;
    int ennemiesNumber = 0;
    int[] ennemiesNumberLimit = { 2, 1, 3, 4 };

    [HideInInspector] public Animator animator;

    public BossBehavior bossBehavior;

    Vector2 lookDir;

    public BossSoundManager bossSoundManager;

    public AllTimelineController shadowCloneCinematic;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        dashAttackCollider.enabled = false;

        initialLife = pv;


        pierresParent = transform.parent.parent.Find("Autres");

        animator = GetComponentInChildren<Animator>();
    }

    override public void FixedUpdate()
    {
        #region Animator

        lookDir = player.transform.position - transform.position;

        for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
        {
            outlineController.outLinesAnimator[i].SetFloat("Horizontal", lookDir.x);
            outlineController.outLinesAnimator[i].SetFloat("Vertical", lookDir.y);
        }

        #endregion 

        missingLife = pv / initialLife;

        if (missingLife <= 0f / 5f /*&& endGameInitiator.ending == false*/)
        {
            //endGameInitiator.StartCoroutine(endGameInitiator.EndGame());
            endCinematic.SetActive(true);

            for (int i = 0; i < parentEnnemies.childCount; i++)
            {
                if (parentEnnemies.GetChild(i).gameObject.activeInHierarchy == true)
                {
                    if (parentEnnemies.GetChild(i).GetComponent<BossBehavior>() != this)
                        parentEnnemies.gameObject.SetActive(false);
                }
            }


        }
            
        if(spawned == false)
        {
            if (missingLife < 1f / 5f)
            {
                if (phase != 5)
                {
                    DestroyShadowClones();
                    StartCoroutine(NextPhase(0, false));
                }

                phase = 5;
            }
            else if (missingLife < 2f / 5f)
            {
                if (phase != 4)
                {
                    StartCoroutine(NextPhase(6, false));
                }

                phase = 4;
            }
            else if (missingLife < 3f / 5f)
            {
                if (phase != 3)
                {
                    //CreateShadowClone();
                    StartCoroutine(NextPhase(6, true));
                }

                phase = 3;
            }
            else if (missingLife <= 4f / 5f)
            {
                if (phase != 2)
                {
                    StartCoroutine(NextPhase(0, false));
                }

                phase = 2;
            }
            else if (missingLife <= 5f / 5f)
                phase = 1;
        }
        

        if (playerProjected)
        {
            direction = Vector2.zero;

            attacking = false;

            timerCooldownPattern = cooldownsPatterns[phase - 1];
        }

        base.FixedUpdate();

        ennemiesNumber = 0;

        for(int i = 0; i < parentEnnemies.childCount; i++)
        {
            if (parentEnnemies.GetChild(i).gameObject.activeInHierarchy == true)
            {
                ennemiesNumber += 1;
            }
        }

        if (projected)
        {
            playerDetected = true;
        }

        if (stuned == true || projected == true)
        {
            canMove = true;
            return;
        }

        if (currentElement != null)
        {
            currentElementBullet = currentElement.GetComponentInChildren<Bullet_Versatil_Controller>();

            if (currentElementBullet != null)
            {
                if (currentElementBullet.levelProjecting >= 2)
                {
                    for (int x = 0; x < currentElement.collider2Ds.Count; x++)
                    {
                        Physics2D.IgnoreCollision(currentElement.collider2Ds[x], col, false);
                    }

                    launched = false;
                    currentElement = null;

                    timerCooldownPattern = 0;
                }
                else
                {
                    player.GetComponent<Player_Main_Controller>().stunned = false;
                    currentElement.StopCoroutine(currentElement.lastTakeForce);

                    for (int i = 0; i < currentElement.collider2Ds.Count; i++)
                    {
                        currentElement.collider2Ds[i].gameObject.layer = currentElement.ennemyCollidersLayers[i];
                    }

                    Destroy(currentElementBullet.gameObject);
                }
            }
            else if (launched)
            {
                
                launched = false;
                currentElement = null;

                timerCooldownPattern = 0;
            }
        }

        if (attacking == false)
        {


            if (timerCooldownPattern > cooldownsPatterns[phase - 1])
            {
                attacking = true;

                switch (phase)
                {
                    case 1:
                        int rand = Random.Range(1, 3);
                        switch (rand)
                        {
                            case 1:
                                rand = Random.Range(1, 4);
                                lastAttack = StartCoroutine(RockThrow(2, 500f, 2f, 30f, rand));
                                break;

                            case 2:
                                if (ennemiesNumber < ennemiesNumberLimit[phase - 1])
                                    lastAttack = StartCoroutine(Minions(1, 2, 2f, 1));
                                else
                                {
                                    attacking = false;
                                }
                                break;
                        }
                        break;

                    case 2:
                        int rand2 = Random.Range(1, 2);
                        switch (rand2)
                        {
                            case 1:
                                lastAttack = StartCoroutine(DashAttack(2, 1000f, 4f, 50f));
                                break;

                            /*case 2:
                                lastAttack = StartCoroutine(RockThrow(3, 500f, 2f, 70f, 2));
                                break;*/

                                /*case 3:
                                    lastAttack = StartCoroutine(SpectralLiane(2, 150f, 10f, 800f));
                                    break;

                                case 4:
                                    if (ennemiesNumber < ennemiesNumberLimit[phase - 1])
                                        lastAttack = StartCoroutine(Minions(2, 1, 1f, 1));
                                    else
                                    {
                                        attacking = false;
                                    }
                                    break;*/
                        }

                        break;

                    case 3:
                        int rand3 = Random.Range(1, 2);
                        switch (rand3)
                        {
                            /*case 1:
                                lastAttack = StartCoroutine(DashAttack(2, 1000f, 5f, 50f));
                                break;*/

                            /*case 1:
                                lastAttack = StartCoroutine(RockThrow(1, 500f, 2f, 45f, 3));
                                break;*/

                            case 1:
                                lastAttack = StartCoroutine(SpectralLiane(2, 500f, 10f, 1200f));
                                break;

                            /*case 2:
                                if (ennemiesNumber < ennemiesNumberLimit[phase - 1])
                                    lastAttack = StartCoroutine(Minions(2, 2, 1f, 1));
                                else
                                {
                                    attacking = false;
                                }
                                break;*/
                        }

                        break;

                    case 4:
                        int rand4 = Random.Range(1, 3);
                        switch (rand4)
                        {
                            case 1:
                                lastAttack = StartCoroutine(SpectralLiane(1, 500f, 10f, 1200f));
                                break;

                            case 2:
                                if (ennemiesNumber < ennemiesNumberLimit[phase - 1])
                                    lastAttack = StartCoroutine(Minions(1, 2, 1f, 1));
                                else
                                {
                                    attacking = false;
                                }
                                break;
                        }

                        break;

                    case 5:
                        int rand5 = Random.Range(1, 4);
                        switch (rand5)
                        {
                            case 1:
                                lastAttack = StartCoroutine(DashAttack(1, 1000f, 4f, 50f));
                                break;

                                case 2:
                                rand = Random.Range(1, 4);
                                lastAttack = StartCoroutine(RockThrow(1, 1000f, 2f, 50f, rand));
                                break;

                                case 3:
                                    lastAttack = StartCoroutine(SpectralLiane(1, 1000f, 10f, 1600f));
                                    break;

                                /*case 4:
                                    if (ennemiesNumber < ennemiesNumberLimit[phase - 1])
                                        lastAttack = StartCoroutine(Minions(2, 1, 1f, 1));
                                    else
                                    {
                                        attacking = false;
                                    }
                                    break;*/
                        }
                        break;

                    case 6:
                        int rand6 = Random.Range(1, 2);
                        switch (rand6)
                        {
                            case 1:
                                lastAttack = StartCoroutine(DashAttack(2, 500f, 4f, 50f));
                                break;

                                /*case 2:
                                    lastAttack = StartCoroutine(RockThrow(3, 500f, 2f, 70f, 2));
                                    break;*/

                                /*case 3:
                                    lastAttack = StartCoroutine(SpectralLiane(2, 150f, 10f, 800f));
                                    break;

                                case 4:
                                    if (ennemiesNumber < ennemiesNumberLimit[phase - 1])
                                        lastAttack = StartCoroutine(Minions(2, 1, 1f, 1));
                                    else
                                    {
                                        attacking = false;
                                    }
                                    break;*/
                        }
                        break;
                }
            }
            else
            {
                timerCooldownPattern += Time.deltaTime;
            }

        }

        if (canMove == true)
        {
            rb.velocity = direction * speed * Time.fixedDeltaTime;
            animator.SetBool("IsFloating", true);
        }
        else
        {
            for (int i = 0; i < outlineController.outLinesAnimator.Count; i++)
            {
                outlineController.outLinesAnimator[i].SetBool("IsFloating", false);
            }
        }

    }



    void DestroyShadowClones()
    {
        
        bossBehavior.gameObject.SetActive(false);
    }

    IEnumerator ResetPattern()
    {
        rb.velocity = Vector2.zero;

        direction = Vector2.zero;

        attacking = false;

        timerCooldownPattern = cooldownsPatterns[phase - 1];

        transform.position = (Vector2)centerTeleport.position + new Vector2(Random.Range(-0.5f, +0.5f), Random.Range(-0.5f, +0.5f));

        for (float i = 0.75f; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

    }

    IEnumerator DashAttack(int numbPattern ,float movementSpeed, float range, float forceValue)
    {

        for(int i = 0; i < numbPattern; i ++)
        {
            bool movingToAttack = true;

            speed = movementSpeed;

            float debugTimer = debugTimerCooldownPattern;

            while (movingToAttack == true)
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= range - 1)
                {
                    direction = (transform.position - player.transform.position).normalized;
                }
                else if (Vector2.Distance(transform.position, player.transform.position) >= range + 1)
                {
                    direction = (player.transform.position - transform.position).normalized;
                }
                else
                {
                    movingToAttack = false;

                    direction = player.transform.position - transform.position;
                }

                if (projected == false)
                    debugTimer -= Time.deltaTime;

                if (debugTimer < 0)
                {
                    StartCoroutine(ResetPattern());

                    yield break;
                }

                yield return null;

            }

            canMove = false;
            
            telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50f));

            for (float x = 0.25f; x > 0; x -= Time.deltaTime)
            {
                rb.velocity = -direction.normalized * 50f * Time.fixedDeltaTime;

                yield return null;
            }

            StartCoroutine(HitboxAttackActivatedFor(1.5f));
            
            rb.velocity = new Vector2(0, 0);

            rb.AddForce((direction).normalized * forceValue, ForceMode2D.Impulse);
            projected = true;
            StartCoroutine(audiomanager.PlayOne("Boss_Dash", gameObject));


            for (int z = 0; z < outlineController.outLinesAnimator.Count; z++)
            {
                outlineController.outLinesAnimator[z].SetTrigger("Jump_Charge");
            }

            while (projected == true)
            {
                yield return null;
            }

            Physics2D.IgnoreCollision(physicCollider, player.physicCollider, false);

            canMove = true;

        }

        direction = Vector2.zero;

        timerCooldownPattern = 0;

        attacking = false;

        yield break;
    }

    public IEnumerator HitboxAttackActivatedFor(float time)
    {
        Physics2D.IgnoreCollision(physicCollider, player.physicCollider, true);
        dashAttackCollider.enabled = true;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Physics2D.IgnoreCollision(physicCollider, player.physicCollider, false);
        dashAttackCollider.enabled = false;
    }

    IEnumerator RockThrow(int numbPattern, float movementSpeed, float range, float forceValue, int caisseMass)
    {
        for(int i = 0; i < numbPattern; i++)
        {
            bool movingToAttack = true;

            speed = movementSpeed;

            List<Elements_Controller> elementsControllers = new List<Elements_Controller>();

            for(int x = 0; x < ennemyControllersList.Count; x++)
            {
                if(ennemyControllersList[x].GetComponent<BossBehavior>() == null)
                    elementsControllers.Add(ennemyControllersList[x]);
            }

            for (int x = 0; x < caisseControllersList.Count; x++)
            {
                elementsControllers.Add(caisseControllersList[x]);
            }

            elementsControllers = elementsControllers.OrderBy(
            x => Vector2.Distance(transform.position, x.transform.position)
            ).ToList();

            float debugTimer = debugTimerCooldownPattern;

            while (movingToAttack == true)
            {
                yield return null;

                if (elementsControllers.Count != 0)
                {

                    if (Vector2.Distance(transform.position, elementsControllers[0].transform.position) <= range - 1)
                    {
                        direction = (transform.position - elementsControllers[0].transform.position).normalized;
                    }
                    else if (Vector2.Distance(transform.position, elementsControllers[0].transform.position) >= range + 1)
                    {
                        direction = (elementsControllers[0].transform.position - transform.position).normalized;
                    }
                    else
                    {
                        currentElement = elementsControllers[0];

                        movingToAttack = false;

                        for (int z = 0; z < outlineController.outLinesAnimator.Count; z++)
                        {
                            outlineController.outLinesAnimator[z].SetTrigger("IsThrowingRock");
                        }

                        direction = (player.transform.position - transform.position).normalized;
                    }
                }
                else
                {
                    currentElement = Instantiate(caissesToSpawns[caisseMass - 1], pierresParent);

                    player.confiner.activeRoom.objectsToDestroy.Add(currentElement);

                    movingToAttack = false;

                    for (int z = 0; z < outlineController.outLinesAnimator.Count; z++)
                    {
                        outlineController.outLinesAnimator[z].SetTrigger("IsThrowingRock");
                    }

                    direction = player.transform.position - transform.position;
                    break;
                }

                if(projected == false)
                    debugTimer -= Time.deltaTime;

                if (debugTimer < 0)
                {
                    if (debugTimer < 0)
                    {
                        StartCoroutine(ResetPattern());

                        yield break;
                    }
                }

            }

            canMove = false;

            direction = Vector2.zero;

            rb.velocity = Vector2.zero;

            //StartCoroutine(CollisionIgnoreWithElement(1.5f));

            for (int x = 0; x < currentElement.collider2Ds.Count; x++)
            {
                Physics2D.IgnoreCollision(currentElement.collider2Ds[x], col, true);
            }

            for (float x = 0.25f; x > 0; x -= Time.deltaTime * 1f)
            {
                if (currentElement == null)
                {
                    attacking = false;

                    direction = Vector2.zero;

                    canMove = true;

                    timerCooldownPattern = 0;

                    StopCoroutine(lastAttack);

                    yield break;
                }

                currentElement.transform.position = (Vector2)transform.position + ((Vector2)(player.transform.position) - (Vector2)transform.position).normalized;
                yield return null;
            }

            Vector2 directionAttack = ((Vector2)(player.transform.position) - (Vector2)transform.position);

            telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50f));

            for (float x = 1.2f; x > 0.7; x -= Time.deltaTime * 1f)
            {
                if (currentElement == null)
                {
                    attacking = false;

                    direction = Vector2.zero;

                    canMove = true;

                    timerCooldownPattern = 0;

                    StopCoroutine(lastAttack);

                    yield break;
                }

                currentElement.transform.position = (Vector2)transform.position + directionAttack.normalized * x;
                yield return null;
            }

            launched = true;
            StartCoroutine(audiomanager.PlayOne("Enemy2_Attack", gameObject));
            currentElement.projected = true;
            currentElement.rb.AddForce(directionAttack.normalized * (forceValue + (forceValue * currentElement.mass)), ForceMode2D.Impulse);

            currentElement.levelProjected = 2f;

            StartCoroutine(CollisionIgnoreWithElement(currentElement, 1f));

            yield return new WaitForSeconds(0.4f);

            canMove = true;
        }

        direction = Vector2.zero;

        attacking = false;

        timerCooldownPattern = 0;

        yield break;
    }

    public IEnumerator CollisionIgnoreWithElement(Elements_Controller ec, float time)
    {
        for (int x = 0; x < ec.collider2Ds.Count; x++)
        {
            Physics2D.IgnoreCollision(ec.collider2Ds[x], col, true);
        }

        while (time > 0)
        {
            for (int x = 0; x < ec.collider2Ds.Count; x++)
            {
                Physics2D.IgnoreCollision(ec.collider2Ds[x], col, true);
            }

            time -= Time.deltaTime;
            yield return null;
        }

        if(ec != null)
        for (int x = 0; x < ec.collider2Ds.Count; x++)
        {
            Physics2D.IgnoreCollision(ec.collider2Ds[x], col, false);
        }
    }

    IEnumerator SpectralLiane(int numbPattern, float movementSpeed, float range, float forceValue)
    {

        for (int i = 0; i < numbPattern; i++)
        {
            bool movingToAttack = true;

            speed = movementSpeed;

            float debugTimer = debugTimerCooldownPattern;

            while (movingToAttack == true)
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= range - 1)
                {
                    direction = (transform.position - player.transform.position).normalized;
                }
                else if (Vector2.Distance(transform.position, player.transform.position) >= range + 1)
                {
                    direction = (player.transform.position - transform.position).normalized;
                }
                else
                {
                    movingToAttack = false;

                    for (int z = 0; z < outlineController.outLinesAnimator.Count; z++)
                    {
                        outlineController.outLinesAnimator[z].SetTrigger("IsUsingLiane");
                    }

                    direction = (player.transform.position - transform.position).normalized;
                }

                if (projected == false)
                    debugTimer -= Time.deltaTime;

                if (debugTimer < 0)
                {
                    if (debugTimer < 0)
                    {
                        StartCoroutine(ResetPattern());

                        yield break;
                    }
                }

                yield return null;
            }

            stuned = true;

            telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50f));

            for (float x = 0.25f; x > 0; x -= Time.deltaTime)
            {
                yield return null;
            }
            StartCoroutine(audiomanager.PlayOne("Enemy3Attack", gameObject));
            BulletAttack1 bullet = Instantiate(bulletAttack1, player.vBullets).GetComponent<BulletAttack1>();

            Collider2D[] ennemyColliders = GetComponentsInChildren<Collider2D>();

            for (int x = 0; x < ennemyColliders.Length - 1; x++)
            {
                Physics2D.IgnoreCollision(ennemyColliders[x], bullet.GetComponentInChildren<Collider2D>(), true);
            }

            bullet.transform.position = transform.position;

            //projected = true;
            //rb.velocity = new Vector2(0, 0);

            bullet.ennemyController = transform;
            bullet.maxDistance = 15f;

            bullet.rb.velocity = (player.transform.position - transform.position).normalized * forceValue * Time.fixedDeltaTime;

            while(stuned == true)
            {
                yield return null;
            }

        }

        direction = Vector2.zero;

        attacking = false;

        timerCooldownPattern = 0;

        yield break;
    }

    public IEnumerator ApplyForce(Player_Main_Controller player)
    {
        stuned = true;

        player.projected = true;

        Vector2 direction = transform.position - player.transform.position;

        int originalPlayerLayer = player.physicCollider.gameObject.layer;

        player.physicCollider.gameObject.layer = 15;

        //StartCoroutine(DontCollideWithPlayerFor(1f));

        while (direction.magnitude > 1.5f)
        {
            direction = (transform.position - player.transform.position);
            player.rb.velocity = direction.normalized * forceSpectralLiane * Time.fixedDeltaTime;
            yield return null;
        }

        stuned = false;

        if (player.GetComponentInChildren<BulletAttack1>() != null)
            Destroy(player.GetComponentInChildren<BulletAttack1>().gameObject);

        player.rb.velocity = new Vector2(0, 0);

        player.rb.AddForce(direction.normalized * forceSpectralLianePlayer / 3f, ForceMode2D.Impulse);

        player.physicCollider.gameObject.layer = originalPlayerLayer;

        StartCoroutine(DontCollideWithPlayerFor(0.5f));
    }

    public IEnumerator ApplyForce(Elements_Controller elementsController)
    {
        stuned = true;

        elementsController.projected = true;

        Vector2 direction = transform.position - elementsController.transform.position;

        for (int i = 0; i < elementsController.collider2Ds.Count; i++)
        {
            elementsController.collider2Ds[i].gameObject.layer = 15;
        }

        while (direction.magnitude > 1.5f)
        {
            direction = (transform.position - elementsController.transform.position);
            elementsController.rb.velocity = direction.normalized * forceSpectralLiane * Time.fixedDeltaTime;
            yield return null;
        }

        stuned = false;

        if (elementsController.GetComponentInChildren<BulletAttack1>() != null)
            Destroy(elementsController.GetComponentInChildren<BulletAttack1>().gameObject);

        elementsController.rb.velocity = new Vector2(0, 0);

        elementsController.rb.AddForce(direction.normalized * forceSpectralLiane / 10f, ForceMode2D.Impulse);

        elementsController.levelProjected = 2;

        for (int i = 0; i < elementsController.collider2Ds.Count; i++)
        {
            elementsController.collider2Ds[i].gameObject.layer = elementsController.ennemyCollidersLayers[i];
        }

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            for (int x = 0; x < elementsController.collider2Ds.Count; x++)
                Physics2D.IgnoreCollision(collider2Ds[i], elementsController.collider2Ds[x], true);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < collider2Ds.Count; i++)
        {
            for (int x = 0; x < elementsController.collider2Ds.Count; x++)
                Physics2D.IgnoreCollision(collider2Ds[i], elementsController.collider2Ds[x], false);
        }
    }

    IEnumerator Minions(int numbPattern, int spawnNumb, float spawnSpeed, int ennemyType)
    {
        for(int i = 0; i < numbPattern; i++)
        {
            Transform teleportPointToGo = null;

            for (int z = 0; z < teleportPoints.Count; z++)
            {
                if(teleportPointToGo != null)
                {
                    if(Vector2.Distance(player.transform.position, teleportPoints[z].position) > Vector2.Distance(player.transform.position, teleportPointToGo.position))
                    {
                        teleportPointToGo = teleportPoints[z];
                    }
                }
                else
                {
                    teleportPointToGo = teleportPoints[z];
                }
            }

                transform.position = (Vector2)teleportPointToGo.position + new Vector2(Random.Range(-0.5f, +0.5f), Random.Range(-0.5f, +0.5f));
                StartCoroutine(audiomanager.PlayOne("Charging", gameObject));

            for (int z = 0; z < outlineController.outLinesAnimator.Count; z++)
            {
                outlineController.outLinesAnimator[z].SetBool("IsSummoning", true);
            }

            yield return new WaitForSeconds(spawnSpeed);

            for(int x = 0; x < spawnNumb; x++)
            {
                Ennemy_Controller currentEnnemy = Instantiate(ennemiesToSpawn[ennemyType - 1], parentEnnemies).GetComponent<Ennemy_Controller>();
                StartCoroutine(audiomanager.PlayOne("Invocation", gameObject));
                player.confiner.activeRoom.objectsToDestroy.Add(currentEnnemy);

                currentEnnemy.transform.position = (Vector2)spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position + new Vector2(Random.Range(-0.5f, +0.5f), Random.Range(-0.5f, +0.5f));

                currentEnnemy.playerDetected = true;

                currentEnnemy.spawned = true;
            }

            yield return new WaitForSeconds(0.3f);
        }

        direction = Vector2.zero;

        attacking = false;

        for (int z = 0; z < outlineController.outLinesAnimator.Count; z++)
        {
            outlineController.outLinesAnimator[z].SetBool("IsSummoning", false);
        }

        timerCooldownPattern = 0;

        yield break;
    }

    IEnumerator NextPhase(int shadowClonePhase, bool createShadowClone)
    {
        for (int z = 0; z < outlineController.outLinesAnimator.Count; z++)
        {
            outlineController.outLinesAnimator[z].SetTrigger("Transitionne");
        }
        StartCoroutine(ResetPattern());

        stuned = true;

        player.speed /= 6f;

        telegraphAttack.StartCoroutine(telegraphAttack.FlashLight(50f));

        bossBehavior.stuned = true;

        for (float x = 0.5f; x > 0; x -= Time.deltaTime)
        {
            yield return null;
        }

        if(currentGlobalLightChanger != null)
        {
            Destroy(currentGlobalLightChanger);
        }

        currentGlobalLightChanger = Instantiate(phasesGlobalLightChangers[phase - 1]);

        currentGlobalLightChanger.transform.position = transform.parent.position;

        for (int i = 0; i < parentEnnemies.childCount; i++)
        {
            Ennemy_Controller ec = parentEnnemies.GetChild(i).GetComponent<Ennemy_Controller>();

            if(ec.gameObject.activeInHierarchy == true && ec.GetComponent<BossBehavior>() == false)
            {
                float playerBossAngle = Vector2.Angle( (Vector2)(ec.transform.position - transform.position).normalized, (Vector2)transform.right);

                if ((ec.transform.position - transform.position).normalized.y < 0)
                {
                    playerBossAngle = -playerBossAngle;
                }
                ec.StartTakeDamage(8f, playerBossAngle);
            }
        }

        for (float x = 0.5f; x > 0; x -= Time.deltaTime)
        {
            yield return null;
        }

        player.TakeForce((player.transform.position - transform.position).normalized, 200f);

        player.speed *= 6f;

        for (float x = 1f; x > 0; x -= Time.deltaTime)
        {
            yield return null;
        }

        bossBehavior.stuned = false;
        bossBehavior.phase = shadowClonePhase;

        stuned = false;

        if(createShadowClone)
        {
            shadowCloneCinematic.gameObject.SetActive(true);
            shadowCloneCinematic.alreadyPlayed = false;
            shadowCloneCinematic.wasPlayed = false;

            shadowCloneCinematic.LaunchTimeline(player);

            StartCoroutine(StunedForSeconds(shadowCloneCinematic.cineTime));

            StartCoroutine(player.StunnedFor(shadowCloneCinematic.cineTime));

            bossBehavior.phase = shadowClonePhase;
        }
    }

    public override IEnumerator Attack1()
    {

        yield break;
    }

}
