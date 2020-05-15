using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public List<SceneTransition> allTransitions;

    public static List<SceneTransition> transitions;

    public static PlayerData transitionData = null;
    public static int previousTansitionID;

    private void Awake()
    {
        transitions = allTransitions;
    }


    public static void Transit(int nextScene, int transitionID)
    {
        previousTansitionID = transitionID;
        transitionData = new PlayerData(GameObject.Find("Player").GetComponent<Player_Main_Controller>());
        SceneManager.LoadScene(nextScene);
    }
}
