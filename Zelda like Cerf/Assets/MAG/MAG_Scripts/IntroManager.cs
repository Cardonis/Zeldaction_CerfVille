using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject intro;
    public AnimationClip logoRubikaClip;

    bool stratingToFade;

    float timerLogo;
    float timer;

    public Animator fondAnimator;

    private void Start()
    {
        timerLogo = logoRubikaClip.length;
        stratingToFade = false;
    }
    private void Update()
    {
       timer += Time.deltaTime;
        if(timer> timerLogo)
        {
            menu.SetActive(true);
            stratingToFade = true;
            fondAnimator.SetBool("StartFading", stratingToFade);
        }
    }
}
