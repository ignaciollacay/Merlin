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

    public bool dialogueEnd;


    public void StartDialogue(DialogueSO dialogue)
    {
        dialogueEnd = false; // FIXME Added only to finish scene.

        Debug.Log("Starting dialogue with " + dialogue.character);
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
        //Debug.Log(sentence);
        dialogue.text = sentence;
    }

    private void EndDialogue()
    {
        //Debug.Log("End of dialogue");
        box.SetActive(false);
        dialogueEnd = true; // FIXME Added only to finish scene.
        FindObjectOfType<CastManager>().NextSpell(); //FIXME Shouldn't be here.
    }
}
