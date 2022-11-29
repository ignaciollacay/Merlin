using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText;

    private Queue<string> sentences = new Queue<string>();

    public UnityEvent DialogueEnd;

    public void StartDialogue(DialogueSO dialogueSO)
    {
        print("startDialogue");
        //sentences.Clear();

        foreach (string sentence in dialogueSO.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        print("displayNextSentence. Count=" + sentences.Count);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    private void EndDialogue()
    {
        print("DialogueEnd");
        DialogueEnd?.Invoke();
    }
}
