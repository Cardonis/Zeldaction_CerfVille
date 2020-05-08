using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDisplay : MonoBehaviour
{
    public GameObject saving;

    public void Start()
    {
        saving.SetActive(false);
    }

    public IEnumerator DisplayFor(float time)
    {
        saving.SetActive(true);

        for (float i = time; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        saving.SetActive(false);
    }
}
