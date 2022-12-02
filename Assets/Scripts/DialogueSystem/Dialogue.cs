using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Se confunde un poco con el Manager en funcionalidad. Por momentos esta manejando el dialogo...
public class Dialogue : MonoBehaviour
{
    public DialogueSO dialogueSO; // TODO: make private, add method GetDialogues(), and refactor DialogueManager;

    [SerializeField] private DialogueHandler dialogueHandler;
    [SerializeField] private bool playOnAwake; // Control only from scene manager or timeline?

    public void TriggerStartDialogue()
    {
        dialogueHandler.StartDialogue(dialogueSO);
    }
    public void TriggerNextDialogue()
    {
        dialogueHandler.DisplayNextSentence();
    }

    public void SetDialogue(DialogueSO newDialogueSO)
    {
        dialogueSO = newDialogueSO;
    }

    public void StartNewDialogue(DialogueSO newDialogueSO)
    {
        SetDialogue(newDialogueSO);
        TriggerStartDialogue();
    }

    private void Awake()
    {
        if (playOnAwake)
            TriggerStartDialogue();
    }
}
