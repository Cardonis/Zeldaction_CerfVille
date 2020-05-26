using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public bool dialogueStarted;
    public Animator animator;
    bool sentenceFullyDisplay;

    public Player_Main_Controller playerscript;

    public GameObject XtoContinue;
    public AudioManager audioManager;

    public Cinemachine.CinemachineVirtualCamera cmVcam;
    private Queue<string> sentences;

    public DialogueManager dialogueManager;

    public bool skip = false;
    [HideInInspector] public bool inSentence=false;
    void Start()
    {
        sentenceFullyDisplay = true;
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (dialogueStarted == true)
        {
            audioManager.Stop("Deplacement");
            cmVcam.m_Lens.OrthographicSize = Mathf.SmoothStep(cmVcam.m_Lens.OrthographicSize, 5f, 3.5f * Time.fixedDeltaTime);

            if (sentenceFullyDisplay == false && Input.GetButtonDown("X") && inSentence == true)
                dialogueManager.skip = true;

            if (sentenceFullyDisplay == true)
            {
                XtoContinue.SetActive(true);
                if (Input.GetButtonDown("X"))
                {
                    DisplayNextSentence();
                }
            }
          
        }
    }
    public void StartDialogue (Dialogue dialogue)
    {
        playerscript.stunned = true;
        nameText.text = dialogue.npcName;
        sentences.Clear();
        dialogueStarted = true;
        animator.SetBool("IsOpen", true);
        

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        string currentsentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentsentence));
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartCoroutine("EndDialogue");
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(0.1f);

        playerscript.stunned = false;
        dialogueStarted = false;
        sentences.Clear();
        animator.SetBool("IsOpen", false);


        for(float i = 3f; i > 0; i -= Time.deltaTime)
        {
            cmVcam.m_Lens.OrthographicSize = Mathf.SmoothStep(cmVcam.m_Lens.OrthographicSize, 7.5f, 20 * Time.deltaTime);

            yield return null;
        }
        
    }

    IEnumerator TypeSentence (string sentence)
    {
        XtoContinue.SetActive(false);
        sentenceFullyDisplay = false;
        dialogueText.text = "";
        dialogueManager.skip = false;
        inSentence = true;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            if ( dialogueManager.skip == false)
            {
                yield return new WaitForSeconds(0.02f);
               
            }
            else
            {

                yield return null;
            }
        }
        inSentence = false;
        dialogueManager.skip = false;
        sentenceFullyDisplay = true;

    }

}
