using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public DialogueSO dialogueSO; // TODO: make private, add method GetDialogues(), and refactor DialogueManager;

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private bool playOnAwake;

    public void TriggerStartDialogue()
    {
        print("triggerDialogue");
        dialogueManager.StartDialogue(dialogueSO);
    }
    public void TriggerNextDialogue()
    {
        print("triggerDialogue");
        dialogueManager.DisplayNextSentence();
    }

    public void SetDialogue(DialogueSO newDialogueSO)
    {
        dialogueSO = newDialogueSO;
    }

    private void Awake()
    {
        if (playOnAwake)
            TriggerStartDialogue();
    }
}
