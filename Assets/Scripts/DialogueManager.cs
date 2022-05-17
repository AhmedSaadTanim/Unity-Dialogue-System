using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText, dialogueText, continueText;
    public Animator animator;
    private Queue<string> sentences;
    private string currentSentence = null;
    [SerializeField] float textSpeed;

    public void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        StopAllCoroutines();


        if (currentSentence != null && dialogueText.text != currentSentence)
        {
            dialogueText.text = currentSentence;
            continueText.text = "CONTINUE";
            if (sentences.Count == 0)
            {
                continueText.text = "DONE";
            }
        }
        else if (sentences.Count != 0)
        {
            currentSentence = sentences.Dequeue();
            StartCoroutine(TypingNextSentence(currentSentence));

        }
        else
        {
            EndDialogue();
            return;
        }
    }

    IEnumerator TypingNextSentence(string sentence)
    {
        dialogueText.text = "";
        continueText.text = "SKIP";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        continueText.text = "CONTINUE";
    }
    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        currentSentence = null;
    }
}
