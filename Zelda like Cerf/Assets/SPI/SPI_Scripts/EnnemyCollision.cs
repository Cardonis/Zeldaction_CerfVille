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

        Caisse_Controller cc = collision.GetComponentInParent<Caisse_Controller>();
        if (cc != null)
        {
            ennemyController.caisseControllersList.Add(cc);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        Ennemy_Controller ec = collision.GetComponentInParent<Ennemy_Controller>();
        if (ec != null)
        {
            ennemyController.ennemyControllersList.Remove(ec);
        }

        Caisse_Controller cc = collision.GetComponentInParent<Caisse_Controller>();
        if (cc != null)
        {
            ennemyController.caisseControllersList.Remove(cc);
        }
    }

}
