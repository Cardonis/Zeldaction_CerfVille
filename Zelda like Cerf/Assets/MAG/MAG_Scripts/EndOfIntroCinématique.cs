using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfCinematique : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        StartCoroutine(LaunchGame());
    }
    IEnumerator LaunchGame()
    {
        animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(1);

        yield break;
    }
}