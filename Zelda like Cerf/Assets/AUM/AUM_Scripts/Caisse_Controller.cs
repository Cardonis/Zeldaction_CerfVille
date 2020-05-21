using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caisse_Controller : Elements_Controller
{
    private bool isPlaying;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        audiomanager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        isTractable = true;
        isPlaying = false;
        StartCoroutine(PlayOneOne());
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
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

    public IEnumerator PlayOneOne()
    {
        isPlaying = true;
        yield return new WaitForSecondsRealtime(1);
        isPlaying = false;
    }

}
