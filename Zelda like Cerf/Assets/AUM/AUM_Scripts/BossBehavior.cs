using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossBehavior : Ennemy_Controller
{
    Vector2 directionPattern;
    public int phase = 1;
    float timerCooldownPattern;
    public RoomController bossRoom;
    float missingLife;

    float[] cooldownsPatterns = { 1.5f, 1f, 0.5f, 1f };

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
    float forceSpectralLiane = 40f;

    [Header("Minions")]

    public List<GameObject> ennemiesToSpawn;
    public Transform parentEnnemies;
    int[] ennemiesNumber = { 3, 1, 3, 3 };


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        dashAttackCollider.enabled = false;

        initialLife = pv;


        pierresParent = transform.parent.parent.Find("Autres");
    }

    override public void FixedUpdate()
    {
        missingLife = pv / initialLife;

        if (missingLife < 1f / 4f)
            phase = 4;
        else if (missingLife < 2f / 4f)
            phase = 3;
        else if (missingLife < 3f / 4f)
            phase = 2;
        else if (missingLife < 4f / 4f)
            phase = 1;

        if (playerProjected)
        {
            direction = Vector2.zero;

            attacking = false;

            timerCooldownPattern = cooldownsPatterns[phase - 1];
        }

        base.FixedUpdate();

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
                    Destroy(currentElementBullet.gameObject);
                }
            }
            else if (launched)
            {
                for (int x = 0; x < currentElement.collider2Ds.Count; x++)
                {
                    Physics2D.IgnoreCollision(currentElement.collider2Ds[x], col, false);
                }

                launched = false;
                currentElement = null;

                timerCooldownPattern = 0;
            }
        }

        if (attacking == false)
        {


            if(timerCooldownPattern > cooldownsPatterns[phase - 1])
            {
                attacking = true;

                switch (phase)
                {
                    case 1:
                        int rand = Random.Range(1, 3);
                        switch (rand)
                        {
                            case 1:
                                lastAttack = StartCoroutine(RockThrow(1, 500f, 2f, 45f, 1));
                                break;

                            case 2:
                                if(ennemyControllersList.Count < ennemiesNumber[phase - 1])
                                    lastAttack = StartCoroutine(Minions(2, 2, 1.5f, 1));
                                else
                                {
                                    attacking = false;
                                }
                                break;
                        }
                        break;

                    case 2:
                        int rand2 = Random.Range(1, 4);
                        switch (rand2)
                        {
                            case 1:
                                lastAttack = StartCoroutine(DashAttack(2, 250f, 5f, 50f));
                                break;

                            case 2:
                                lastAttack = StartCoroutine(SpectralLiane(2, 150f, 10f, 800f));
                                break;

                            case 3:
                                if (ennemyControllersList.Count < ennemiesNumber[phase - 1])
                                    lastAttack = StartCoroutine(Minions(2, 1, 1f, 1));
                                else
                                {
                                    attacking = false;
                                }
                                break;
                        }

                        break;

                    case 3:
                        int rand3 = Random.Range(1, 4);
                        switch (rand3)
                        {
                            case 1:
                                lastAttack = StartCoroutine(DashAttack(2, 500f, 5f, 50f));
                                break;

                            case 2:
                                lastAttack = StartCoroutine(RockThrow(1, 500f, 2f, 45f, 1));
                                break;

                            case 3:
                                lastAttack = StartCoroutine(SpectralLiane(2, 300f, 10f, 800f));
                                break;

                            case 4:
                                if (ennemyControllersList.Count < ennemiesNumber[phase - 1])
                                    lastAttack = StartCoroutine(Minions(1, 2, 0.2f, 1));
                                else
                                {
                                    attacking = false;
                                }
                                break;
                        }

                        break;

                    case 4:
                        int rand4 = Random.Range(1, 4);
                        switch (rand4)
                        {
                            case 1:
                                lastAttack = StartCoroutine(DashAttack(2, 500f, 5f, 50f));
                                break;

                            case 2:
                                lastAttack = StartCoroutine(RockThrow(1, 500f, 2f, 45f, 1));
                                break;

                            case 3:
                                if (ennemyControllersList.Count < ennemiesNumber[phase - 1])
                                    lastAttack = StartCoroutine(Minions(2, 1, 0.5f, 1));
                                else
                                {
                                    attacking = false;
                                }
                                break;
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
        }

    }

    IEnumerator DashAttack(int numbPattern ,float movementSpeed, float range, float forceValue)
    {

        for(int i = 0; i < numbPattern; i ++)
        {
            bool movingToAttack = true;

            speed = movementSpeed;

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

                yield return null;

            }

            canMove = false;

            for (float x = 0.25f; x > 0; x -= Time.deltaTime)
            {
                rb.velocity = -direction.normalized * 50f * Time.fixedDeltaTime;

                yield return null;
            }

            StartCoroutine(HitboxAttackActivatedFor(1.5f));

            rb.velocity = new Vector2(0, 0);

            rb.AddForce((direction).normalized * forceValue, ForceMode2D.Impulse);
            projected = true;

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
                elementsControllers.Add(ennemyControllersList[x]);
            }

            for (int x = 0; x < caisseControllersList.Count; x++)
            {
                elementsControllers.Add(caisseControllersList[x]);
            }

            elementsControllers = elementsControllers.OrderBy(
            x => Vector2.Distance(transform.position, x.transform.position)
            ).ToList();

            while (movingToAttack == true)
            {
                if(elementsControllers.Count != 0)
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

                        direction = (player.transform.position - transform.position).normalized;
                    }
                }
                else
                {
                    currentElement = Instantiate(caissesToSpawns[caisseMass - 1], pierresParent);

                    movingToAttack = false;

                    direction = player.transform.position - transform.position;
                    break;
                }

                yield return null;

            }

            canMove = false;

            //StartCoroutine(CollisionIgnoreWithElement(1.5f));

            for (int x = 0; x < currentElement.collider2Ds.Count; x++)
            {
                Physics2D.IgnoreCollision(currentElement.collider2Ds[x], col, true);
            }

            for (float x = 0.4f; x > 0; x -= Time.deltaTime)
            {
                if (currentElement == null)
                {
                    attacking = false;

                    direction = Vector2.zero;

                    canMove = true;

                    timerCooldownPattern = 0;

                    StopCoroutine(lastAttack);

                    break;
                }

                currentElement.transform.position = transform.position + (player.transform.position - transform.position).normalized * 1.2f;

                yield return null;
            }

            Vector2 directionAttack = (player.transform.position - transform.position).normalized;

            for (float x = 1.2f; x > 0.7; x -= Time.deltaTime * 1f)
            {
                if (currentElement == null)
                {
                    for (int z = 0; z < currentElement.collider2Ds.Count; z++)
                    {
                        Physics2D.IgnoreCollision(currentElement.collider2Ds[z], col, false);
                    }

                    attacking = false;

                    direction = Vector2.zero;

                    canMove = true;

                    timerCooldownPattern = 0;

                    StopCoroutine(lastAttack);

                    break;
                }

                currentElement.transform.position = (Vector2)transform.position + directionAttack * x;
                yield return null;
            }

            currentElement.projected = true;
            currentElement.rb.AddForce(directionAttack * forceValue, ForceMode2D.Impulse);

            currentElement.levelProjected = 2f;

            StartCoroutine(CollisionIgnoreWithElement(0.1f));

            yield return new WaitForSeconds(0.4f);

            canMove = true;
        }

        direction = Vector2.zero;

        attacking = false;

        timerCooldownPattern = 0;

        yield break;
    }

    public IEnumerator CollisionIgnoreWithElement(float time)
    {
        for (int x = 0; x < currentElement.collider2Ds.Count; x++)
        {
            Physics2D.IgnoreCollision(currentElement.collider2Ds[x], col, true);
        }

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        for (int x = 0; x < currentElement.collider2Ds.Count; x++)
        {
            Physics2D.IgnoreCollision(currentElement.collider2Ds[x], col, false);
        }
    }

    IEnumerator SpectralLiane(int numbPattern, float movementSpeed, float range, float forceValue)
    {

        for (int i = 0; i < numbPattern; i++)
        {
            bool movingToAttack = true;

            speed = movementSpeed;

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

                    direction = (player.transform.position - transform.position).normalized;
                }

                yield return null;
            }

            stuned = true;

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

        while (direction.magnitude > 0.7f)
        {
            direction = (transform.position - player.transform.position);
            player.rb.velocity = direction.normalized * forceSpectralLiane;
            yield return null;
        }

        stuned = false;

        if (player.GetComponentInChildren<BulletAttack1>() != null)
            Destroy(player.GetComponentInChildren<BulletAttack1>().gameObject);

        player.rb.velocity = new Vector2(0, 0);

        player.rb.AddForce(direction.normalized * forceSpectralLiane * 3, ForceMode2D.Impulse);

        player.physicCollider.gameObject.layer = originalPlayerLayer;

        StartCoroutine(DontCollideWithPlayerFor(0.1f));
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

        while (direction.magnitude > 0.7f)
        {
            direction = (transform.position - elementsController.transform.position);
            elementsController.rb.velocity = direction.normalized * forceSpectralLiane;
            yield return null;
        }

        stuned = false;

        if (elementsController.GetComponentInChildren<BulletAttack1>() != null)
            Destroy(elementsController.GetComponentInChildren<BulletAttack1>().gameObject);

        elementsController.rb.velocity = new Vector2(0, 0);

        elementsController.rb.AddForce(direction.normalized * forceSpectralLiane * 3, ForceMode2D.Impulse);

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

        yield return new WaitForSeconds(0.1f);

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
            transform.position = (Vector2)bossRoom.transform.position + new Vector2(Random.Range(-10, +10), Random.Range(-10, +10));

            yield return new WaitForSeconds(spawnSpeed);

            for(int x = 0; x < spawnNumb; x++)
            {
                Ennemy_Controller currentEnnemy = Instantiate(ennemiesToSpawn[ennemyType - 1], parentEnnemies).GetComponent<Ennemy_Controller>();

                currentEnnemy.transform.position = (Vector2)bossRoom.transform.position + new Vector2(Random.Range(-10, +10), Random.Range(-10, +10));

                currentEnnemy.playerDetected = true;
            }

            yield return new WaitForSeconds(0.3f);
        }

        direction = Vector2.zero;

        attacking = false;

        timerCooldownPattern = 0;

        yield break;
    }

    public override IEnumerator Attack1()
    {

        yield break;
    }
}
