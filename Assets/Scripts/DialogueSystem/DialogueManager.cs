using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject box; // Should replace with anim
    [SerializeField] private Text character;
    [SerializeField] private Text dialogue;

    private Queue<string> sentences = new Queue<string>();

    public bool dialogueEnd = false;


    public void StartDialogue(DialogueSO dialogue)
    {
        dialogueEnd = false;

        box.SetActive(true);
        character.text = dialogue.character;


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

    private void EndDialogue()
    {
        box.SetActive(false);
        dialogueEnd = true;
    }
}
