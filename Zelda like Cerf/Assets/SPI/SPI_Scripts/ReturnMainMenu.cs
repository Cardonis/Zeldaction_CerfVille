using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnMainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("X") && Input.GetButtonDown("A"))
        {
            ReturnToMainMenu();
        }
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
