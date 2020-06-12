using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameInitiator : MonoBehaviour
{
    public Player_Main_Controller player;
    [HideInInspector] public bool ending = false;

    public IEnumerator EndGame()
    {
        ending = true;

        yield return new WaitForSeconds(1f);

        player.fadeAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(3);
    }
    public void OnEnable()
    {
        StartCoroutine(EndGame());
    }
}
