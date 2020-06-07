using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPlayer : MonoBehaviour
{
    public bool isInTips=false;
    public GameObject tipsMark;
    public GameObject tipsRenv;
    public GameObject tipsYoyo;
    public GameObject cadre;

    public GameObject currentTips;
    public Image fadeScreen;
    public Player_Main_Controller playerscript;
    private void Update()
    {
        if ((Input.GetButtonDown("X") || Input.GetButtonDown("A") || Input.GetButtonDown("Y")) && isInTips)
        {
            StartCoroutine(CloseTips());
        }
    }
    public IEnumerator OpenTips(GameObject tips)
    {
        float elapsedTime = 0.0f;
        Time.timeScale = 0f;
        fadeScreen.gameObject.SetActive(true);
        Color c = fadeScreen.color;

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            c.a = 0.0f + Mathf.Clamp01(elapsedTime / (0.5f))*0.7f;
            fadeScreen.color = c;
            yield return null;
        }
        playerscript.stunned = true;
        cadre.SetActive(true);
        tips.SetActive(true);
        currentTips = tips;
        yield return null;
        isInTips = true;

    }
    public IEnumerator CloseTips()
    {
        float elapsedTime = 0.0f;
        isInTips = false;
        fadeScreen.gameObject.SetActive(true);
        Color c = fadeScreen.color;

        playerscript.stunned = false;
        cadre.SetActive(false);
        currentTips.SetActive(false);
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            c.a = 0.7f - Mathf.Clamp01(elapsedTime / (0.5f)*0.7f);
            fadeScreen.color = c;
            yield return null;
        }
        Time.timeScale = 1f;
        yield return null;
    }

    public void tips1Launcher()
    {
        StartCoroutine(OpenTips(tipsMark));
    }

    public void tips2Launcher()
    {
        StartCoroutine(OpenTips(tipsYoyo));
    }

    public void tips3Launcher()
    {
        StartCoroutine(OpenTips(tipsRenv));
    }
}
