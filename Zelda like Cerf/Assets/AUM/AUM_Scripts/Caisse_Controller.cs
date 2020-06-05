using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caisse_Controller : Elements_Controller
{
    private bool isPlaying;
    bool playerNearby = false;
    [HideInInspector] public float initialMass;
    float slowMultiplicator = 100;
    Coroutine lastDisableMassLoss;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        isTractable = true;
        isPlaying = false;
        initialMass = rb.mass;
        StartCoroutine(PlayOneOne());
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (projected)
            return;

        if ((player.transform.position - transform.position).magnitude < 1.5f)
        {
            rb.mass = initialMass * 100f;
        }
        else
        {
            rb.mass = initialMass;
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Elements_Controller ec = collision.transform.GetComponent<Elements_Controller>();

        if (projected == true && levelProjected >= 0.5f && velocityBeforeImpact.magnitude > 5f)
        {
            if((collision.transform.tag == "Wall" || collision.transform.tag == "Ronce") || ec != null)
            {
                if(player.cameraShake.shaking == false)
                    player.cameraShake.lastCameraShake = player.cameraShake.StartCoroutine(player.cameraShake.CameraShakeFor(0.1f, 0.1f, levelProjected));

                if (isPlaying == false)
                {
                    StartCoroutine(audiomanager.PlayOne("Rock_Impact", gameObject));
                    StartCoroutine(PlayOneOne());
                }
            }
           
        }
    }

    /*private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(lastDisableMassLoss != null)
            StopCoroutine(lastDisableMassLoss);

            lastDisableMassLoss = StartCoroutine(DisableMassLossAfter(1f));
        }
    }*/

    public IEnumerator DisableMassLossAfter(float time)
    {
        yield return new WaitForSeconds(time);

        rb.mass = initialMass;
    }

    public IEnumerator PlayOneOne()
    {
        isPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        isPlaying = false;
    }

}
