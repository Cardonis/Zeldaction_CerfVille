using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsPlayer : MonoBehaviour
{
    public bool isInTips=false;
    public GameObject tipsMark;
    public GameObject tipsRenv;
    public GameObject tipsYoyo;

    public GameObject currentTips;
    public Animator fadeAnimator;
    public Player_Main_Controller playerscript;
    private void Update()
    {
        if (Input.GetButtonDown("X") && isInTips)
        {
            StartCoroutine(CloseTips(currentTips));
        }
    }
    public IEnumerator OpenTips(GameObject tips)
    {
        playerscript.stunned = true;
        fadeAnimator.SetTrigger("FadeOut");
        //Time.timeScale = 0f;
        tips.SetActive(true);
        currentTips = tips;
        yield return null;
        isInTips = true;

    }
    public IEnumerator CloseTips(GameObject currentTips)
    {
        playerscript.stunned = false;
        isInTips = false;
        currentTips.SetActive(false);
        fadeAnimator.SetTrigger("FadeIn");
        //Time.timeScale = 1f;
        yield return null;
    }
}
