using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [HideInInspector] public bool outLining;

    public Color outlineColor;

    public GameObject baseGraph;

    public List<SpriteRenderer> outLines;
    public List<Animator> outLinesAnimator;

    public float thiccness;

    // Start is called before the first frame update
    void Start()
    {
        int outLinesSortingOrder = baseGraph.GetComponent<SpriteRenderer>().sortingOrder - 1;

        Animator baseGraphAnimator = baseGraph.GetComponent<Animator>();

        SpriteRenderer topOutline = Instantiate(baseGraph, transform).GetComponent<SpriteRenderer>();

        topOutline.color = outlineColor;
        topOutline.sortingOrder = outLinesSortingOrder;
        topOutline.transform.localPosition = new Vector3(topOutline.transform.localPosition.x, topOutline.transform.localPosition.y + thiccness);
        outLines.Add(topOutline);

        SpriteRenderer leftOutline = Instantiate(baseGraph, transform).GetComponent<SpriteRenderer>();

        leftOutline.color = outlineColor;
        leftOutline.sortingOrder = outLinesSortingOrder;
        leftOutline.transform.localPosition = new Vector3(leftOutline.transform.localPosition.x - thiccness, leftOutline.transform.localPosition.y);
        outLines.Add(leftOutline);

        SpriteRenderer downOutline = Instantiate(baseGraph, transform).GetComponent<SpriteRenderer>();

        downOutline.color = outlineColor;
        downOutline.sortingOrder = outLinesSortingOrder;
        downOutline.transform.localPosition = new Vector3(downOutline.transform.localPosition.x, downOutline.transform.localPosition.y - thiccness);
        outLines.Add(downOutline);

        SpriteRenderer rightOutline = Instantiate(baseGraph, transform).GetComponent<SpriteRenderer>();

        rightOutline.color = outlineColor;
        rightOutline.sortingOrder = outLinesSortingOrder;
        rightOutline.transform.localPosition = new Vector3(rightOutline.transform.localPosition.x + thiccness, rightOutline.transform.localPosition.y);
        outLines.Add(rightOutline);

        if(baseGraphAnimator != null)
        {
            outLinesAnimator.Add(baseGraphAnimator);
            outLinesAnimator.Add(topOutline.GetComponent<Animator>());
            outLinesAnimator.Add(leftOutline.GetComponent<Animator>());
            outLinesAnimator.Add(downOutline.GetComponent<Animator>());
            outLinesAnimator.Add(rightOutline.GetComponent<Animator>());
        }

        outLining = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(outLining == true)
        {
            for(int i = 0; i < outLines.Count; i++)
            {
                outLines[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < outLines.Count; i++)
            {
                outLines[i].enabled = false;
            }
        }
    }
}
