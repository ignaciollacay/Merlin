using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] public DialogueSO dialogueSO;
    [SerializeField] private DialogueManager dialogueManager;

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogueSO);
    }
}
