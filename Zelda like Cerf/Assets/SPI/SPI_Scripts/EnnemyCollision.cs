using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyCollision : MonoBehaviour
{

    public Ennemy_Controller ennemyController;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Ennemy_Controller ec = collision.GetComponentInParent<Ennemy_Controller>();
        if (ec != null)
        {
            ennemyController.ennemyControllersList.Add(ec);

        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        Ennemy_Controller ec = collision.GetComponentInParent<Ennemy_Controller>();
        if (ec != null)
        {
            ennemyController.ennemyControllersList.Remove(ec);
        }
    }

}
