using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject box; // FIXME: Remove. // Should replace with anim 
    [SerializeField] private Text dialogue;

    private Queue<string> sentences = new Queue<string>();

    public bool dialogueEnd = false;

    public UnityEvent DialogueEnd;

    public void StartDialogue(DialogueSO dialogue)
    {
        dialogueEnd = false;

        box.SetActive(true);


        //sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// Run from button on click event. Shows the next line if sentences remain, else runs end dialogue.
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogue.text = sentence;
    }

    // TODO: EVENT
    private void EndDialogue()
    {
        box.SetActive(false);
        dialogueEnd = true;
        DialogueEnd?.Invoke();
    }
}
