using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyCollision : MonoBehaviour
{

    public Ennemy_Controller ennemyController;

    private void Update()
    {
        for(int i = 0; i < ennemyController.ennemyControllersList.Count; i++)
        {
            if( ennemyController.ennemyControllersList[i].gameObject.activeSelf == false )
            {
                ennemyController.ennemyControllersList.Remove(ennemyController.ennemyControllersList[i]);
            }
        }

        for (int i = 0; i < ennemyController.caisseControllersList.Count; i++)
        {
            if (ennemyController.caisseControllersList[i].gameObject.activeSelf == false)
            {
                ennemyController.caisseControllersList.Remove(ennemyController.caisseControllersList[i]);
            }
        }
    }

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
