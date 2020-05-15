using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Transform spawnPos;
    public int nextScene;
    public int transitionID;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player_Main_Controller player = collider.GetComponentInParent<Player_Main_Controller>();
        if(player != null)
        {
            TransitionManager.Transit(nextScene, transitionID);
        }
    }
}
