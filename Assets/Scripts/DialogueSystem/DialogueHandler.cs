using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private Text dialogueText;

    private Queue<string> sentences = new Queue<string>();

    public UnityEvent DialogueEnd;

    public void StartDialogue(DialogueSO dialogueSO)
    {
        // TODO: This shouldn't be commented. Check if it works removing comment
        //sentences.Clear();

        foreach (string sentence in dialogueSO.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
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
        DialogueEnd?.Invoke();
    }

    public void ClearAllListeners()
    {
        DialogueEnd.RemoveAllListeners();
    }

    public void PlayAfterDialogueEnd(PlayableDirector playableDirector)
    {
        DialogueEnd.AddListener(() => playableDirector.Play());
    }
}
